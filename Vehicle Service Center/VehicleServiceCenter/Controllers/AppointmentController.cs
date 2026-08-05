
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
    
}