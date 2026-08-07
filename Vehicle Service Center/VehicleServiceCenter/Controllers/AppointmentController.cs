using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VehicleServiceCenter.Models;
using VehicleServiceCenter;

namespace VehicleServiceCenter.Controllers;

[ApiController]
[Route("[controller]")]
public class AppointmentController : Controller
{
    private readonly ProjectContext _context;

    public AppointmentController(ProjectContext context)
    {
        _context = context;
        
    }
    [HttpGet]
    public IActionResult GetAppointments()
    {
        return  Ok(_context.Appointments.ToList());
    }
    [HttpGet("{id}")]
    public IActionResult GetAppointment(int id)
    {
        var IsAppointmentFound =  _context.Appointments.Equals(id);

        if (!IsAppointmentFound)
            return NotFound();

        var appointmen = _context.Appointments.Find(id);
        return Ok(appointmen);
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

        _context.SaveChanges();

        return NoContent();
    }
    [HttpDelete("{id}")]
    public IActionResult DeleteAppointment(int id)
    {
        // check if it is exits 1
        AppointmentModel appointment=  _context.Appointments.Find(id);

        if (appointment == null )
            return NotFound();
        
        // delete  3
        _context.Appointments.Remove(appointment);
        
        // save 4
        _context.SaveChanges();

        return NoContent();
    }
    
    
}