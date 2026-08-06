using Microsoft.AspNetCore.Mvc;
using System.Linq;
using VehicleServiceCenter.Models;

namespace VehicleServiceCenter.Controllers
{
    [ApiController]
    [Route("Vehicle")]
    public class VehicleController : ControllerBase
    {
        private readonly ProjectContext context;

        public VehicleController(ProjectContext context)
        {
            this.context = context;
        }

        // Register a new vehicle
        [HttpPost("RegisterVehicle")]
        public IActionResult RegisterVehicle(VehicleModel vehicle)
        {
            // Check whether the plate number already exists
            VehicleModel existingPlate = context.Vehicles
                .FirstOrDefault(v => v.PlateNumber == vehicle.PlateNumber);

            if (existingPlate != null)
            {
                return BadRequest("Plate number is already registered");
            }

            // Check whether the VIN already exists (VIN is optional)
            if (!string.IsNullOrEmpty(vehicle.VIN))
            {
                VehicleModel existingVin = context.Vehicles
                    .FirstOrDefault(v => v.VIN == vehicle.VIN);

                if (existingVin != null)
                {
                    return BadRequest("VIN is already registered");
                }
            }

            vehicle.CreatedAt = DateTime.Now;

            context.Vehicles.Add(vehicle);
            context.SaveChanges();

            return Ok(new
            {
                Message = "Vehicle registered successfully",
                VehicleId = vehicle.VehicleId
            });
        }

        // Get all vehicles
        [HttpGet("GetAll")]
        public IActionResult GetAllVehicles()
        {
            var vehicles = context.Vehicles.ToList();
            return Ok(vehicles);
        }

        // Get vehicle by ID
        [HttpGet("GetById/{id}")]
        public IActionResult GetVehicleById(int id)
        {
            var vehicle = context.Vehicles.Find(id);
            if (vehicle == null)
            {
                return NotFound("Vehicle not found");
            }
            return Ok(vehicle);
        }

        // Get all vehicles belonging to a specific customer
        [HttpGet("GetByCustomer/{customerProfileId}")]
        public IActionResult GetVehiclesByCustomer(int customerProfileId)
        {
            var vehicles = context.Vehicles
                .Where(v => v.CustomerProfileId == customerProfileId)
                .ToList();

            return Ok(vehicles);
        }

        // Update vehicle by ID
        [HttpPut("Update/{id}")]
        public IActionResult UpdateVehicle(int id, VehicleModel updatedVehicle)
        {
            var vehicle = context.Vehicles.Find(id);
            if (vehicle == null)
            {
                return NotFound("Vehicle not found");
            }

            // Prevent duplicate plate number when it's being changed
            if (vehicle.PlateNumber != updatedVehicle.PlateNumber)
            {
                bool plateTaken = context.Vehicles
                    .Any(v => v.PlateNumber == updatedVehicle.PlateNumber && v.VehicleId != id);

                if (plateTaken)
                {
                    return BadRequest("Plate number is already registered");
                }
            }

            // Prevent duplicate VIN when it's being changed
            if (!string.IsNullOrEmpty(updatedVehicle.VIN) && vehicle.VIN != updatedVehicle.VIN)
            {
                bool vinTaken = context.Vehicles
                    .Any(v => v.VIN == updatedVehicle.VIN && v.VehicleId != id);

                if (vinTaken)
                {
                    return BadRequest("VIN is already registered");
                }
            }

            vehicle.CustomerProfileId = updatedVehicle.CustomerProfileId;
            vehicle.PlateNumber = updatedVehicle.PlateNumber;
            vehicle.VIN = updatedVehicle.VIN;
            vehicle.Make = updatedVehicle.Make;
            vehicle.Model = updatedVehicle.Model;
            vehicle.Year = updatedVehicle.Year;
            vehicle.Color = updatedVehicle.Color;
            vehicle.Mileage = updatedVehicle.Mileage;

            context.SaveChanges();

            return Ok(new
            {
                Message = "Vehicle updated successfully",
                VehicleId = vehicle.VehicleId
            });
        }

        // Delete vehicle by ID
        [HttpDelete("Delete/{id}")]
        public IActionResult DeleteVehicle(int id)
        {
            var vehicle = context.Vehicles.Find(id);
            if (vehicle == null)
            {
                return NotFound("Vehicle not found");
            }

            context.Vehicles.Remove(vehicle);
            context.SaveChanges();

            return Ok(new
            {
                Message = "Vehicle deleted successfully",
                VehicleId = vehicle.VehicleId
            });
        }
    }
}
