using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VehicleServiceCenter.Models;
using VehicleServiceCenter.Services;

namespace VehicleServiceCenter.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceOrderItemController : ControllerBase
    {
        private ProjectContext _context;
        private readonly IResourceAuthorizationService _resourceAccess;

        public ServiceOrderItemController(
            ProjectContext context,
            IResourceAuthorizationService resourceAccess)
        {
            _context = context;
            _resourceAccess = resourceAccess;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ServiceOrderItemModel>>> GetAll()
        {
            return await _resourceAccess
                .ScopeServiceOrderItems(_context.ServiceOrderItems)
                .Include(i => i.ServiceType)
                .Include(i => i.SparePart)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceOrderItemModel>> GetById(int id)
        {
            var item = await _resourceAccess
                .ScopeServiceOrderItems(_context.ServiceOrderItems)
                .Include(i => i.ServiceType)
                .Include(i => i.SparePart)
                .FirstOrDefaultAsync(i => i.ServiceOrderItemId == id);

            if (item == null) return NotFound();
            return item;
        }

        [HttpPost]
        public async Task<ActionResult<ServiceOrderItemModel>> Create(ServiceOrderItemModel item)
        {
            if (!_resourceAccess.CanManageServiceOrder(item.ServiceOrderId))
                return Forbid();

            string? validationError = await ValidateItem(item);
            if (validationError != null) return BadRequest(validationError);

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

            if (!_resourceAccess.CanManageServiceOrderItem(id))
                return Forbid();

            string? validationError = await ValidateItem(updated);
            if (validationError != null) return BadRequest(validationError);

            item.ItemType = updated.ItemType;
            item.ServiceTypeId = updated.ServiceTypeId;
            item.SparePartId = updated.SparePartId;
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

            if (!_resourceAccess.CanManageServiceOrderItem(id))
                return Forbid();

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

            if (!_resourceAccess.CanManageServiceOrderItem(id))
                return Forbid();

            if (quantity <= 0)
                return BadRequest("Quantity must be greater than zero.");

            item.Quantity = quantity;
            item.Subtotal = quantity * item.UnitPrice;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // Case 7: filter by parent order or item type
        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<ServiceOrderItemModel>>> Filter([FromQuery] int? serviceOrderId, [FromQuery] string? itemType)
        {
            var query = _resourceAccess
                .ScopeServiceOrderItems(_context.ServiceOrderItems)
                .Include(i => i.ServiceType)
                .Include(i => i.SparePart)
                .AsQueryable();
            if (serviceOrderId.HasValue) query = query.Where(i => i.ServiceOrderId == serviceOrderId.Value);
            if (!string.IsNullOrEmpty(itemType)) query = query.Where(i => i.ItemType == itemType);
            return await query.ToListAsync();
        }

        // Case 8: sum subtotal per parent order
        [HttpGet("total/{serviceOrderId}")]
        public async Task<ActionResult> GetOrderTotal(int serviceOrderId)
        {
            if (!_resourceAccess.CanAccessServiceOrder(serviceOrderId))
                return Forbid();

            var total = await _resourceAccess
                .ScopeServiceOrderItems(_context.ServiceOrderItems)
                .Where(i => i.ServiceOrderId == serviceOrderId)
                .SumAsync(i => i.Subtotal);
            return Ok(new { ServiceOrderId = serviceOrderId, Total = total });
        }

        private async Task<string?> ValidateItem(ServiceOrderItemModel item)
        {
            if (item.Quantity <= 0) return "Quantity must be greater than zero.";
            if (item.UnitPrice < 0) return "Unit price cannot be negative.";

            if (item.ItemType == "Service")
            {
                if (!item.ServiceTypeId.HasValue || item.SparePartId.HasValue)
                    return "A service item must select one service type and no spare part.";

                if (!await _context.ServiceTypes.AnyAsync(service =>
                        service.ServiceTypeId == item.ServiceTypeId.Value))
                    return "The selected service type does not exist.";
            }
            else if (item.ItemType == "SparePart")
            {
                if (!item.SparePartId.HasValue || item.ServiceTypeId.HasValue)
                    return "A spare-part item must select one spare part and no service type.";

                if (!await _context.SpareParts.AnyAsync(part =>
                        part.SparePartId == item.SparePartId.Value))
                    return "The selected spare part does not exist.";
            }
            else
            {
                return "Item type must be Service or SparePart.";
            }

            return null;
        }
    }
}
