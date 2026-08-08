using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VehicleServiceCenter.Models;

namespace VehicleServiceCenter.Controllers;

[ApiController]
[Route("[controller]")]
public class AppointmentController : ControllerBase
{
    private readonly ProjectContext context;

    public AppointmentController(ProjectContext context)
    {
        this.context = context;
    }

    // 1. GET - Get all appointments with related entities
    [HttpGet]
    public async Task<IActionResult> GetAppointments()
    {
        var appointments = await context.Appointments
            .Include(a => a.CustomerProfile)
            .Include(a => a.Vehicle)
            .Include(a => a.ServiceType)
            .Include(a => a.MechanicProfile)
            .Include(a => a.Branch)
            .ToListAsync();

        return Ok(appointments);
    }

    // 2. GET - Get appointment by ID
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAppointment(int id)
    {
        var appointment = await context.Appointments
            .Include(a => a.CustomerProfile)
            .Include(a => a.Vehicle)
            .Include(a => a.ServiceType)
            .Include(a => a.MechanicProfile)
            .Include(a => a.Branch)
            .FirstOrDefaultAsync(a => a.AppointmentId == id);

        if (appointment == null)
            return NotFound();

        return Ok(appointment);
    }

    // 3. POST - Create appointment
    [HttpPost]
    public async Task<ActionResult<AppointmentModel>> CreateAppointment(
        AppointmentModel appointmentModel)
    {
        context.Appointments.Add(appointmentModel);

        await context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetAppointment),
            new { id = appointmentModel.AppointmentId },
            appointmentModel);
    }

    // 4. PUT - Update appointment
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAppointment(
        int id,
        AppointmentModel appointmentModel)
    {
        if (id != appointmentModel.AppointmentId)
            return BadRequest("ID does not match.");

        var existingAppointment = await context.Appointments
            .FindAsync(id);

        if (existingAppointment == null)
            return NotFound();

        existingAppointment.CustomerProfileId =
            appointmentModel.CustomerProfileId;

        existingAppointment.VehicleId =
            appointmentModel.VehicleId;

        existingAppointment.ServiceTypeId =
            appointmentModel.ServiceTypeId;

        existingAppointment.MechanicProfileId =
            appointmentModel.MechanicProfileId;

        existingAppointment.BranchId =
            appointmentModel.BranchId;

        existingAppointment.AppointmentDate =
            appointmentModel.AppointmentDate;

        existingAppointment.Status =
            appointmentModel.Status;

        existingAppointment.Notes =
            appointmentModel.Notes;

        await context.SaveChangesAsync();

        return NoContent();
    }

    // 5. DELETE - Delete appointment
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAppointment(int id)
    {
        var appointment = await context.Appointments.FindAsync(id);

        if (appointment == null)
            return NotFound();

        context.Appointments.Remove(appointment);

        await context.SaveChangesAsync();

        return NoContent();
    }

    // 6. GET - Filter appointments by status
    [HttpGet("filter/{status}")]
    public async Task<IActionResult> GetAppointmentsByStatus(string status)
    {
        var appointments = await context.Appointments
            .Include(a => a.CustomerProfile)
            .Include(a => a.Vehicle)
            .Include(a => a.ServiceType)
            .Where(a => a.Status == status)
            .ToListAsync();

        return Ok(appointments);
    }

    // 7. GET - Sort appointments by date
    [HttpGet("sorted")]
    public async Task<IActionResult> GetSortedAppointments()
    {
        var appointments = await context.Appointments
            .Include(a => a.CustomerProfile)
            .Include(a => a.Vehicle)
            .OrderBy(a => a.AppointmentDate)
            .ToListAsync();

        return Ok(appointments);
    }

    // 8. PATCH - Change appointment status
    [HttpPatch("{id}/status")]
    public async Task<IActionResult> ChangeAppointmentStatus(
        int id,
        [FromBody] string status)
    {
        var appointment = await context.Appointments.FindAsync(id);

        if (appointment == null)
            return NotFound();

        appointment.Status = status;

        await context.SaveChangesAsync();

        return Ok(appointment);
    }
}