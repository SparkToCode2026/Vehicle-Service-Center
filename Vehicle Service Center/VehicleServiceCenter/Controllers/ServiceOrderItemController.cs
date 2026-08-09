using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VehicleServiceCenter.Models;

namespace VehicleServiceCenter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceOrderItemController: ControllerBase
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
    }
}