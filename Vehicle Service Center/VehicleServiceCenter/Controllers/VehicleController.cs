using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VehicleServiceCenter.Models;

namespace VehicleServiceCenter.Controllers;

[ApiController]
[Route("[controller]")]
public class VehicleController : ControllerBase
{
    private readonly ProjectContext context;

    public VehicleController(ProjectContext context)
    {
        this.context = context;
    }

    // 1. POST - Create a new vehicle
    [HttpPost]
    public IActionResult CreateVehicle(VehicleModel vehicle)
    {
        context.Vehicles.Add(vehicle);
        context.SaveChanges();

        return CreatedAtAction(
            nameof(GetVehicle),
            new { id = vehicle.VehicleId },
            vehicle
        );
    }

    // 2. PUT - Update vehicle details
    [HttpPut("{id}")]
    public IActionResult UpdateVehicle(int id, VehicleModel vehicle)
    {
        var existingVehicle = context.Vehicles.Find(id);

        if (existingVehicle == null)
            return NotFound();

        existingVehicle.Make = vehicle.Make;
        existingVehicle.Model = vehicle.Model;
        existingVehicle.Year = vehicle.Year;
        existingVehicle.PlateNumber = vehicle.PlateNumber;
        existingVehicle.VIN = vehicle.VIN;

        context.SaveChanges();

        return NoContent();
    }

    // 3. PATCH - Reassign vehicle to a different customer (FK change)
    [HttpPatch("{id}/reassign")]
    public IActionResult ReassignVehicle(int id, int customerProfileId)
    {
        var vehicle = context.Vehicles.Find(id);

        if (vehicle == null)
            return NotFound();

        var customerExists = context.CustomerProfiles.Any(c => c.CustomerProfileId == customerProfileId);

        if (!customerExists)
            return BadRequest("CustomerProfileId does not exist.");

        vehicle.CustomerProfileId = customerProfileId;
        context.SaveChanges();

        return Ok(vehicle);
    }

    // 4. DELETE - Delete a vehicle
    [HttpDelete("{id}")]
    public IActionResult DeleteVehicle(int id)
    {
        var vehicle = context.Vehicles.Find(id);

        if (vehicle == null)
            return NotFound();

        var hasServiceOrders = context.ServiceOrders.Any(s => s.VehicleId == id);

        if (hasServiceOrders)
            return Conflict("Cannot delete this vehicle: it has existing service orders.");

        context.Vehicles.Remove(vehicle);
        context.SaveChanges();

        return NoContent();
    }

    // 5. GET - Get all vehicles with CustomerProfile
    [HttpGet]
    public IActionResult GetVehicles()
    {
        var vehicles = context.Vehicles
            .Include(v => v.CustomerProfile)
            .ToList();

        return Ok(vehicles);
    }

    // 6. GET - Get vehicle by ID
    [HttpGet("{id}")]
    public IActionResult GetVehicle(int id)
    {
        var vehicle = context.Vehicles
            .Include(v => v.CustomerProfile)
            .Include(v => v.ServiceOrders)
            .FirstOrDefault(v => v.VehicleId == id);

        if (vehicle == null)
            return NotFound();

        return Ok(vehicle);
    }

    // 7. GET - Filter vehicles by make
    [HttpGet("filter")]
    public IActionResult GetVehiclesByMake(string make)
    {
        var vehicles = context.Vehicles
            .Where(v => v.Make.ToLower() == make.ToLower())
            .Include(v => v.CustomerProfile)
            .ToList();

        return Ok(vehicles);
    }

    // 8. GET - Count vehicles grouped by make (aggregate)
    [HttpGet("summary")]
    public IActionResult GetVehicleCountByMake()
    {
        var summary = context.Vehicles
            .GroupBy(v => v.Make)
            .Select(g => new { Make = g.Key, Count = g.Count() })
            .OrderByDescending(g => g.Count)
            .ToList();

        return Ok(summary);
    }
}
