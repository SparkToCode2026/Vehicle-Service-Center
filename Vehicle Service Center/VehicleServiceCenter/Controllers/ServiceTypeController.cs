using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VehicleServiceCenter.Models;

namespace VehicleServiceCenter.Controllers;

[ApiController]
[Route("[controller]")]
public class ServiceTypeController : ControllerBase
{
    private readonly ProjectContext context;

    public ServiceTypeController(ProjectContext context)
    {
        this.context = context;
    }

    // 1. POST - Create a new service type
    [HttpPost]
    public IActionResult CreateServiceType(ServiceTypeModel serviceType)
    {
        context.ServiceTypes.Add(serviceType);
        context.SaveChanges();

        return CreatedAtAction(
            nameof(GetServiceType),
            new { id = serviceType.ServiceTypeId },
            serviceType
        );
    }

    // 2. PUT - Update service type details
    [HttpPut("{id}")]
    public IActionResult UpdateServiceType(int id, ServiceTypeModel serviceType)
    {
        var existingServiceType = context.ServiceTypes.Find(id);

        if (existingServiceType == null)
            return NotFound();

        existingServiceType.Name = serviceType.Name;
        existingServiceType.Description = serviceType.Description;
        existingServiceType.BasePrice = serviceType.BasePrice;
        existingServiceType.EstimatedDurationMinutes = serviceType.EstimatedDurationMinutes;

        context.SaveChanges();

        return NoContent();
    }

    // 3. PATCH - Activate/deactivate a service type
    [HttpPatch("{id}/status")]
    public IActionResult SetServiceTypeStatus(int id, bool isActive)
    {
        var serviceType = context.ServiceTypes.Find(id);

        if (serviceType == null)
            return NotFound();

        serviceType.IsActive = isActive;
        context.SaveChanges();

        return Ok(serviceType);
    }

    // 4. DELETE - Delete a service type (or soft-delete if it's still referenced)
    [HttpDelete("{id}")]
    public IActionResult DeleteServiceType(int id)
    {
        var serviceType = context.ServiceTypes.Find(id);

        if (serviceType == null)
            return NotFound();

        var isReferenced = context.ServiceOrderItems.Any(i => i.ServiceTypeId == id);

        if (isReferenced)
        {
            serviceType.IsActive = false;
            context.SaveChanges();
            return Ok("Service type is referenced by existing orders, so it was deactivated instead of deleted.");
        }

        context.ServiceTypes.Remove(serviceType);
        context.SaveChanges();

        return NoContent();
    }

    // 5. GET - Get all service types with ServiceOrderItems
    [HttpGet]
    public IActionResult GetServiceTypes()
    {
        var serviceTypes = context.ServiceTypes
            .Include(s => s.ServiceOrderItems)
            .ToList();

        return Ok(serviceTypes);
    }

    // 6. GET - Get service type by ID
    [HttpGet("{id}")]
    public IActionResult GetServiceType(int id)
    {
        var serviceType = context.ServiceTypes
            .Include(s => s.ServiceOrderItems)
            .FirstOrDefault(s => s.ServiceTypeId == id);

        if (serviceType == null)
            return NotFound();

        return Ok(serviceType);
    }

    // 7. GET - Filter service types by active status
    [HttpGet("filter")]
    public IActionResult GetActiveServiceTypes(bool isActive)
    {
        var serviceTypes = context.ServiceTypes
            .Where(s => s.IsActive == isActive)
            .ToList();

        return Ok(serviceTypes);
    }

    
}
