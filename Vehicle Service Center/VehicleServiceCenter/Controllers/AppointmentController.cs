using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VehicleServiceCenter.Models;

namespace VehicleServiceCenter.Controllers;
[Authorize]
[ApiController]
[Route("[controller]")]
public class AppointmentController : ControllerBase
{
    private readonly ProjectContext context;

    public AppointmentController(ProjectContext context)
    {
        this.context = context;
    }

    // 1. POST - Create a new appointment
    [HttpPost]
    public IActionResult CreateAppointment(AppointmentModel appointment)
    {
        appointment.CreatedAt = DateTime.Now;

        context.Appointments.Add(appointment);
        context.SaveChanges();

        return CreatedAtAction(
            nameof(GetAppointment),
            new { id = appointment.AppointmentId },
            appointment
        );
    }

    // 2. PUT - Update appointment details
    [HttpPut("{id}")]
    public IActionResult UpdateAppointment(int id, AppointmentModel appointment)
    {
        var existingAppointment = context.Appointments.Find(id);

        if (existingAppointment == null)
            return NotFound();

        existingAppointment.CustomerProfileId = appointment.CustomerProfileId;
        existingAppointment.VehicleId = appointment.VehicleId;
        existingAppointment.ServiceTypeId = appointment.ServiceTypeId;
        existingAppointment.MechanicProfileId = appointment.MechanicProfileId;
        existingAppointment.BranchId = appointment.BranchId;
        existingAppointment.AppointmentDate = appointment.AppointmentDate;
        existingAppointment.Status = appointment.Status;
        existingAppointment.Notes = appointment.Notes;

        context.SaveChanges();

        return NoContent();
    }

    // 3. PATCH - Change appointment status
    [HttpPatch("{id}/status")]
    public IActionResult ChangeAppointmentStatus(int id, string status)
    {
        var appointment = context.Appointments.Find(id);

        if (appointment == null)
            return NotFound();

        appointment.Status = status;

        context.SaveChanges();

        return Ok(appointment);
    }

    // 4. DELETE - Delete an appointment
    [HttpDelete("{id}")]
    public IActionResult DeleteAppointment(int id)
    {
        var appointment = context.Appointments.Find(id);

        if (appointment == null)
            return NotFound();

        context.Appointments.Remove(appointment);
        context.SaveChanges();

        return NoContent();
    }

    // 5. GET - Get all appointments with related entities
    [HttpGet]
    public IActionResult GetAppointments()
    {
        var appointments = context.Appointments
            .Include(a => a.CustomerProfile)
            .Include(a => a.Vehicle)
            .Include(a => a.ServiceType)
            .Include(a => a.MechanicProfile)
            .Include(a => a.Branch)
            .ToList();

        return Ok(appointments);
    }

    // 6. GET - Get appointment by ID
    [HttpGet("{id}")]
    public IActionResult GetAppointment(int id)
    {
        var appointment = context.Appointments
            .Include(a => a.CustomerProfile)
            .Include(a => a.Vehicle)
            .Include(a => a.ServiceType)
            .Include(a => a.MechanicProfile)
            .Include(a => a.Branch)
            .FirstOrDefault(a => a.AppointmentId == id);

        if (appointment == null)
            return NotFound();

        return Ok(appointment);
    }

    // 7. GET - Filter appointments by status
    [HttpGet("filter")]
    public IActionResult GetAppointmentsByStatus(string status)
    {
        var appointments = context.Appointments
            .Where(a => a.Status == status)
            .Include(a => a.CustomerProfile)
            .Include(a => a.Vehicle)
            .Include(a => a.ServiceType)
            .ToList();

        return Ok(appointments);
    }

    // 8. GET - Sort appointments by date
    [HttpGet("sort")]
    public IActionResult GetAppointmentsSorted()
    {
        var appointments = context.Appointments
            .OrderBy(a => a.AppointmentDate)
            .Include(a => a.CustomerProfile)
            .Include(a => a.Vehicle)
            .Include(a => a.ServiceType)
            .ToList();

        return Ok(appointments);
    }
}

//