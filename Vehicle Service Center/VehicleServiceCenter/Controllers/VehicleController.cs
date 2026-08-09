using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using VehicleServiceCenter.DTOs;
using VehicleServiceCenter.Models;

namespace VehicleServiceCenter.Controllers
{
    [ApiController]
    [Route("Vehicle")]
    public class VehicleController : ControllerBase
    {
        private ProjectContext context;

        public VehicleController(ProjectContext context)
        {
            context = context;
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

        // Create a new vehicle
        [HttpPost]
        public async Task<ActionResult<Vehicle>> CreateVehicle(VehicleCreateDto dto)
        {
            var customerExists = await _context.CustomerProfiles.AnyAsync(c => c.CustomerProfileId == dto.CustomerProfileId);
            if (!customerExists) return BadRequest("CustomerProfileId does not exist.");

            var vehicle = new Vehicle
            {
                CustomerProfileId = dto.CustomerProfileId,
                Make = dto.Make,
                Model = dto.Model,
                Year = dto.Year,
                PlateNumber = dto.PlateNumber,
                VIN = dto.VIN
            };

            _context.Vehicles.Add(vehicle);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetVehicleById), new { id = vehicle.VehicleId }, vehicle);
        }

        // Full update of a vehicle's details
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVehicle(int id, VehicleUpdateDto dto)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle == null) return NotFound();

            vehicle.Make = dto.Make;
            vehicle.Model = dto.Model;
            vehicle.Year = dto.Year;
            vehicle.PlateNumber = dto.PlateNumber;
            vehicle.VIN = dto.VIN;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // Second, distinct update: reassign vehicle to a different customer (FK change)
        [HttpPatch("{id}/reassign")]
        public async Task<IActionResult> ReassignVehicle(int id, VehicleReassignDto dto)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle == null) return NotFound();

            var customerExists = await _context.CustomerProfiles.AnyAsync(c => c.CustomerProfileId == dto.NewCustomerProfileId);
            if (!customerExists) return BadRequest("NewCustomerProfileId does not exist.");

            vehicle.CustomerProfileId = dto.NewCustomerProfileId;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // Delete a vehicle
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

            // Vehicles grouped/counted by make, ordered by count desc
            [HttpGet("by-make-summary")]
            public async Task<ActionResult<IEnumerable<object>>> GetVehicleCountByMake()
            {
                var summary = await _context.Vehicles
                    .GroupBy(v => v.Make)
                    .Select(g => new { Make = g.Key, Count = g.Count() })
                    .OrderByDescending(g => g.Count)
                    .ToListAsync();

                return Ok(summary);
            }
        }
    }
}
