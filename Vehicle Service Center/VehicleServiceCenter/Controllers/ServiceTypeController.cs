using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using VehicleServiceCenter.Models;

namespace VehicleServiceCenter.Controllers;

[Authorize]
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
    [Authorize(Roles = "Admin")]
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
    [Authorize(Roles = "Admin")]
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
    [Authorize(Roles = "Admin")]
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
    [Authorize(Roles = "Admin")]
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
    [AllowAnonymous]
    [HttpGet]
    public IActionResult GetServiceTypes()
    {
        var serviceTypes = context.ServiceTypes
            .Include(s => s.ServiceOrderItems)
            .ToList();

        return Ok(serviceTypes);
    }

    // 6. GET - Get service type by ID
    [AllowAnonymous]
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
    [AllowAnonymous]
    [HttpGet("filter")]
    public IActionResult GetActiveServiceTypes(bool isActive)
    {
        var serviceTypes = context.ServiceTypes
            .Where(s => s.IsActive == isActive)
            .ToList();

        return Ok(serviceTypes);
    }

    // 8. GET - Revenue per service type (aggregate)
    [Authorize(Roles = "Admin")]
    [HttpGet("revenue")]
    public IActionResult GetRevenueByServiceType()
    {
        var summary = context.ServiceTypes
            .Select(s => new
            {
                s.ServiceTypeId,
                s.Name,
                TotalRevenue = s.ServiceOrderItems.Sum(i => (decimal?)(i.UnitPrice * i.Quantity)) ?? 0m
            })
            .OrderByDescending(s => s.TotalRevenue)
            .ToList();

        return Ok(summary);
    }
}
