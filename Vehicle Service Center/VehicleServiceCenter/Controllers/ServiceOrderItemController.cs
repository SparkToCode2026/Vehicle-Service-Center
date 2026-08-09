using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VehicleServiceCenter.Models;

namespace VehicleServiceCenter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceOrderItemController : ControllerBase
    {
        private ProjectContext _context;
        public ServiceOrderItemController(ProjectContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ServiceOrderItemModel>>> GetAll()
        {
            return await _context.ServiceOrderItems
                .Include(i => i.ServiceType)
                .Include(i => i.SparePart)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceOrderItemModel>> GetById(int id)
        {
            var item = await _context.ServiceOrderItems
                .Include(i => i.ServiceType)
                .Include(i => i.SparePart)
                .FirstOrDefaultAsync(i => i.ServiceOrderItemId == id);

            if (item == null) return NotFound();
            return item;
        }

        [HttpPost]
        public async Task<ActionResult<ServiceOrderItemModel>> Create(ServiceOrderItemModel item)
        {
            item.Subtotal = item.Quantity * item.UnitPrice;
            _context.ServiceOrderItems.Add(item);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = item.ServiceOrderItemId }, item);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ServiceOrderItemModel updated)
        {
            var item = await _context.ServiceOrderItems.FindAsync(id);
            if (item == null) return NotFound();

            item.Quantity = updated.Quantity;
            item.UnitPrice = updated.UnitPrice;
            item.LaborHours = updated.LaborHours;
            item.Description = updated.Description;
            item.Subtotal = updated.Quantity * updated.UnitPrice;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.ServiceOrderItems.FindAsync(id);
            if (item == null) return NotFound();

            _context.ServiceOrderItems.Remove(item);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // Case 3: partial update via a distinct route (quantity only), recalculates Subtotal
        [HttpPatch("{id}/quantity")]
        public async Task<IActionResult> UpdateQuantity(int id, [FromBody] int quantity)
        {
            var item = await _context.ServiceOrderItems.FindAsync(id);
            if (item == null) return NotFound();

            item.Quantity = quantity;
            item.Subtotal = quantity * item.UnitPrice;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // Case 7: filter by parent order or item type
        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<ServiceOrderItemModel>>> Filter([FromQuery] int? serviceOrderId, [FromQuery] string? itemType)
        {
            var query = _context.ServiceOrderItems.Include(i => i.ServiceType).Include(i => i.SparePart).AsQueryable();
            if (serviceOrderId.HasValue) query = query.Where(i => i.ServiceOrderId == serviceOrderId.Value);
            if (!string.IsNullOrEmpty(itemType)) query = query.Where(i => i.ItemType == itemType);
            return await query.ToListAsync();
        }

        // Case 8: sum subtotal per parent order
        [HttpGet("total/{serviceOrderId}")]
        public async Task<ActionResult> GetOrderTotal(int serviceOrderId)
        {
            var total = await _context.ServiceOrderItems
                .Where(i => i.ServiceOrderId == serviceOrderId)
                .SumAsync(i => i.Subtotal);
            return Ok(new { ServiceOrderId = serviceOrderId, Total = total });
        }
    }
}