using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VehicleServiceCenter.Models;
using VehicleServiceCenter.Data;

namespace VehicleServiceCenter.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AppointmentController : Controller
{
    private readonly ProjectContext _context;

    public AppointmentController(ProjectContext context)
    {
        _context = context;
        
    }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AppointmentModel>>> GetAppointments()
    {
        return await _context.Appointments.ToListAsync();
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<AppointmentModel>> GetAppointment(int id)
    {
        var appointment = await _context.Appointments.FindAsync(id);

        if (appointment == null)
            return NotFound();

        return appointment;
    }
    [HttpPost]
    public async Task<ActionResult<AppointmentModel>> CreateAppointment(AppointmentModel appointmentModel)
    {
        _context.Appointments.Add(appointmentModel);

        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetAppointment),
            new { id = appointmentModel.AppointmentId }, appointmentModel);
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAppointment(int id, AppointmentModel appointmentModel)
    {
        if (id != appointmentModel.AppointmentId)
            return BadRequest();

        _context.Entry(appointmentModel).State = EntityState.Modified;

        await _context.SaveChangesAsync();

        return NoContent();
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAppointment(int id)
    {
        var appointment = await _context.Appointments.FindAsync(id);

        if (appointment == null)
            return NotFound();

        _context.Appointments.Remove(appointment);

        await _context.SaveChangesAsync();

        return NoContent();
    }
    
    
}