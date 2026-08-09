using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VehicleServiceCenter.Models;
using VehicleServiceCenter.Services;

namespace VehicleServiceCenter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceOrderController : ControllerBase
    {
        private readonly ProjectContext _context;
        private readonly IEmailService _emailService;

        public ServiceOrderController(ProjectContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ServiceOrderModel>>> GetAll()
        {
            return await _context.ServiceOrders
                .Include(so => so.Vehicle)
                .Include(so => so.ServiceOrderItems)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceOrderModel>> GetById(int id)
        {
            var order = await _context.ServiceOrders
                .Include(so => so.Vehicle)
                .Include(so => so.ServiceOrderItems)
                .FirstOrDefaultAsync(so => so.ServiceOrderId == id);

            if (order == null) return NotFound();
            return order;
        }

        [HttpPost]
        public async Task<ActionResult<ServiceOrderModel>> Create(ServiceOrderModel order)
        {
            order.CreatedAt = DateTime.UtcNow;
            order.OrderDate = DateTime.UtcNow;
            _context.ServiceOrders.Add(order);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = order.ServiceOrderId }, order);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ServiceOrderModel updated)
        {
            var order = await _context.ServiceOrders.FindAsync(id);
            if (order == null) return NotFound();

            // capture BEFORE overwriting, so the email only fires on the transition into Completed
            bool justCompleted = order.Status != "Completed" && updated.Status == "Completed";

            order.MechanicProfileId = updated.MechanicProfileId;
            order.Diagnosis = updated.Diagnosis;
            order.Status = updated.Status;
            order.TotalAmount = updated.TotalAmount;
            order.CompletionDate = updated.CompletionDate;

            await _context.SaveChangesAsync();

            if (justCompleted)
            {
                var customerProfile = await _context.CustomerProfiles.FindAsync(order.CustomerProfileId);
                if (customerProfile != null)
                {
                    var user = await _context.Users.FindAsync(customerProfile.UserId);
                    if (user != null && !string.IsNullOrEmpty(user.Email))
                    {
                        await _emailService.SendEmailAsync(
                            user.Email,
                            "Your Vehicle Is Ready for Pickup",
                            $"Hi {user.UserName},\n\n" +
                            $"Your service order #{order.ServiceOrderId} has been completed. " +
                            $"Total amount: {order.TotalAmount:C}.\n\n" +
                            $"Please visit the branch to pick up your vehicle.\n\nThank you!"
                        );
                    }
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var order = await _context.ServiceOrders.FindAsync(id);
            if (order == null) return NotFound();

            _context.ServiceOrders.Remove(order);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}