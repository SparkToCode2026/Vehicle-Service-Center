using Microsoft.AspNetCore.Mvc;
using System.Linq;
using VehicleServiceCenter.Models;

namespace VehicleServiceCenter.Controllers
{
    [ApiController]
    [Route("ServiceType")]
    public class ServiceTypeController : ControllerBase
    {
        private ProjectContext context;

        public ServiceTypeController(ProjectContext context)
        {
            context = context;
        }

        // Register a new service type
        [HttpPost("RegisterServiceType")]
        public IActionResult RegisterServiceType(ServiceTypeModel serviceType)
        {
            // Check whether the service type name already exists
            ServiceTypeModel existingType = context.ServiceTypes
                .FirstOrDefault(s => s.Name == serviceType.Name);

            if (existingType != null)
            {
                return BadRequest("Service type name is already registered");
            }

            serviceType.IsActive = true;

            context.ServiceTypes.Add(serviceType);
            context.SaveChanges();

            return Ok(new
            {
                Message = "Service type registered successfully",
                ServiceTypeId = serviceType.ServiceTypeId
            });
        }

        // Get all service types
        [HttpGet("GetAll")]
        public IActionResult GetAllServiceTypes()
        {
            var serviceTypes = context.ServiceTypes.ToList();
            return Ok(serviceTypes);
        }

        // Get service type by ID
        [HttpGet("GetById/{id}")]
        public IActionResult GetServiceTypeById(int id)
        {
            var serviceType = context.ServiceTypes.Find(id);
            if (serviceType == null)
            {
                return NotFound("Service type not found");
            }
            return Ok(serviceType);
        }

        // Update service type by ID
        [HttpPut("Update/{id}")]
        public IActionResult UpdateServiceType(int id, ServiceTypeModel updatedServiceType)
        {
            var serviceType = context.ServiceTypes.Find(id);
            if (serviceType == null)
            {
                return NotFound("Service type not found");
            }

            // Prevent duplicate name when it's being changed
            if (serviceType.Name != updatedServiceType.Name)
            {
                bool nameTaken = context.ServiceTypes
                    .Any(s => s.Name == updatedServiceType.Name && s.ServiceTypeId != id);

                if (nameTaken)
                {
                    return BadRequest("Service type name is already registered");
                }
            }

            serviceType.Name = updatedServiceType.Name;
            serviceType.Description = updatedServiceType.Description;
            serviceType.BasePrice = updatedServiceType.BasePrice;
            serviceType.EstimatedDurationMinutes = updatedServiceType.EstimatedDurationMinutes;

            context.SaveChanges();

            return Ok(new
            {
                Message = "Service type updated successfully",
                ServiceTypeId = serviceType.ServiceTypeId
            });
        }

        // Change active status by ID
        [HttpPatch("ChangeStatus/{id}")]
        public IActionResult ChangeServiceTypeStatus(int id, bool isActive)
        {
            var serviceType = context.ServiceTypes.Find(id);
            if (serviceType == null)
            {
                return NotFound("Service type not found");
            }

            serviceType.IsActive = isActive;
            context.SaveChanges();

            return Ok(new
            {
                Message = "Service type status changed successfully",
                ServiceTypeId = serviceType.ServiceTypeId,
                IsActive = serviceType.IsActive
            });
        }

        // Delete service type by ID
        [HttpDelete("Delete/{id}")]
        public IActionResult DeleteServiceType(int id)
        {
            var serviceType = context.ServiceTypes.Find(id);
            if (serviceType == null)
            {
                return NotFound("Service type not found");
            }

            context.ServiceTypes.Remove(serviceType);
            context.SaveChanges();

            return Ok(new
            {
                Message = "Service type deleted successfully",
                ServiceTypeId = serviceType.ServiceTypeId
            });
        }
    }
}
