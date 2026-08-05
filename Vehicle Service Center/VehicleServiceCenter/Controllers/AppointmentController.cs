
namespace VehicleServiceCenter.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VehicleServiceCenter.Models;

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
    public async Task<ActionResult<IEnumerable<Appointment>>> GetAppointments()
    {
        return await _context.Appointments.ToListAsync();
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<Appointment>> GetAppointment(int id)
    {
        var appointment = await _context.Appointments.FindAsync(id);

        if (appointment == null)
            return NotFound();

        return appointment;
    }
    [HttpPost]
    public async Task<ActionResult<Appointment>> CreateAppointment(Appointment appointment)
    {
        _context.Appointments.Add(appointment);

        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetAppointment),
            new { id = appointment.AppointmentId }, appointment);
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAppointment(int id, Appointment appointment)
    {
        if (id != appointment.AppointmentId)
            return BadRequest();

        _context.Entry(appointment).State = EntityState.Modified;

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