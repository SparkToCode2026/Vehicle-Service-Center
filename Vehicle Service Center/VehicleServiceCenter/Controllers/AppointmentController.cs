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
    private ProjectContext context;

    public AppointmentController(ProjectContext context)
    {
        context = context;
        
    }
    [HttpGet]
    public IActionResult GetAppointments()
    {
        return  Ok(context.Appointments.ToList());
    }
    [HttpGet("{id}")]
    public IActionResult GetAppointment(int id)
    {
        var IsAppointmentFound =  context.Appointments.Equals(id);

        if (!IsAppointmentFound)
            return NotFound();

        var appointmen = context.Appointments.Find(id);
        return Ok(appointmen);
    }
    [HttpPost]
    public async Task<ActionResult<AppointmentModel>> CreateAppointment(AppointmentModel appointmentModel)
    {
        context.Appointments.Add(appointmentModel);

        await context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetAppointment),
            new { id = appointmentModel.AppointmentId }, appointmentModel);
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAppointment(int id, AppointmentModel appointmentModel)
    {
        if (id != appointmentModel.AppointmentId)
            return BadRequest();

        context.Entry(appointmentModel).State = EntityState.Modified;

        context.SaveChanges();

        return NoContent();
    }
    [HttpDelete("{id}")]
    public IActionResult DeleteAppointment(int id)
    {
        // check if it is exits 1
        AppointmentModel appointment=  context.Appointments.Find(id);

        if (appointment == null )
            return NotFound();
        
        // delete  3
        context.Appointments.Remove(appointment);
        
        // save 4
        context.SaveChanges();

        return NoContent();
    }
    
    
}