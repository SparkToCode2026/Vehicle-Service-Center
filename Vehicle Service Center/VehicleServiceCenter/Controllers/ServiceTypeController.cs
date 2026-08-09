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

    
}