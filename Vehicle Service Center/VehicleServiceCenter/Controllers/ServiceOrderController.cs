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

            order.MechanicProfileId = updated.MechanicProfileId;
            order.Diagnosis = updated.Diagnosis;
            order.TotalAmount = updated.TotalAmount;

            await _context.SaveChangesAsync();
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

        // Case 3: dedicated status-transition endpoint, validates legal transitions before writing
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> ChangeStatus(int id, [FromBody] string newStatus)
        {
            var order = await _context.ServiceOrders.FindAsync(id);
            if (order == null) return NotFound();

            var validTransitions = new Dictionary<string, string[]>
            {
                ["Pending"] = new[] { "Approved", "Cancelled" },
                ["Approved"] = new[] { "InProgress", "Cancelled" },
                ["InProgress"] = new[] { "Completed", "Cancelled" },
            };

            if (!validTransitions.TryGetValue(order.Status, out var allowed) || !allowed.Contains(newStatus))
                return BadRequest($"Cannot transition from {order.Status} to {newStatus}.");

            order.Status = newStatus;
            if (newStatus == "Completed")
            {
                order.CompletionDate = DateTime.UtcNow;
                var customerProfile = await _context.CustomerProfiles.FindAsync(order.CustomerProfileId);
                if (customerProfile != null)
                {
                    var user = await _context.Users.FindAsync(customerProfile.UserId);
                    if (user != null && !string.IsNullOrEmpty(user.Email))
                    {
                        await _emailService.SendEmailAsync(
                            user.Email,
                            "Your Vehicle Is Ready for Pickup",
                            $"Hi {user.UserName},\n\nYour service order #{order.ServiceOrderId} has been completed. Total: {order.TotalAmount:C}.\n\nThank you!"
                        );
                    }
                }
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // Case 7: filter by status and/or date range
        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<ServiceOrderModel>>> Filter([FromQuery] string? status, [FromQuery] DateTime? from, [FromQuery] DateTime? to)
        {
            var query = _context.ServiceOrders.Include(so => so.Vehicle).AsQueryable();
            if (!string.IsNullOrEmpty(status)) query = query.Where(so => so.Status == status);
            if (from.HasValue) query = query.Where(so => so.OrderDate >= from.Value);
            if (to.HasValue) query = query.Where(so => so.OrderDate <= to.Value);
            return await query.ToListAsync();
        }

        // Case 8: group + aggregate by status
        [HttpGet("summary")]
        public async Task<ActionResult> GetSummary()
        {
            var summary = await _context.ServiceOrders
                .GroupBy(so => so.Status)
                .Select(g => new { Status = g.Key, Count = g.Count(), TotalRevenue = g.Sum(so => so.TotalAmount) })
                .ToListAsync();
            return Ok(summary);
        }
    }
}