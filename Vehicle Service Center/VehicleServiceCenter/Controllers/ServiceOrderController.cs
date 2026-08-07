using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VehicleServiceCenter.Models;

namespace VehicleServiceCenter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceOrderController: ControllerBase
    {
        private ProjectContext context;
        public ServiceOrderController(ProjectContext context)
        {
            context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ServiceOrderModel>>> GetAll()
        {
            return await context.ServiceOrders
                .Include(so => so.Vehicle)
                .Include(so => so.ServiceOrderItems)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceOrderModel>> GetById(int id)
        {
            var order = await context.ServiceOrders
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
            context.ServiceOrders.Add(order);
            await context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = order.ServiceOrderId }, order);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ServiceOrderModel updated)
        {
            var order = await context.ServiceOrders.FindAsync(id);
            if (order == null) return NotFound();

            order.MechanicProfileId = updated.MechanicProfileId;
            order.Diagnosis = updated.Diagnosis;
            order.Status = updated.Status;
            order.TotalAmount = updated.TotalAmount;
            order.CompletionDate = updated.CompletionDate;

            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var order = await context.ServiceOrders.FindAsync(id);
            if (order == null) return NotFound();

            context.ServiceOrders.Remove(order);
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}