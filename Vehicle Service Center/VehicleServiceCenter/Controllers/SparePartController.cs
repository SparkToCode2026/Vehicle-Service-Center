using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VehicleServiceCenter.Models;

namespace VehicleServiceCenter.Controllers;

[ApiController]
[Route("[controller]")]
public class SparePartController : ControllerBase
{
    private readonly ProjectContext _context;

    public SparePartController(ProjectContext context)
    {
        _context = context;
    }

    // GET: api/SparePart
    [HttpGet]
    public IActionResult GetSpareParts()
    {
        return Ok(_context.SpareParts.ToList());
    }

    // GET: api/SparePart/5
    [HttpGet("{id}")]
    public IActionResult GetSparePart(int id)
    {
        var sparePart = _context.SpareParts.Find(id);

        if (sparePart == null)
            return NotFound();

        return Ok(sparePart);
    }

    // POST: api/SparePart
    [HttpPost]
    public IActionResult CreateSparePart(SparePartModel sparePart)
    {
        _context.SpareParts.Add(sparePart);
        _context.SaveChanges();

        return CreatedAtAction(nameof(GetSparePart),
            new { id = sparePart.SparePartId }, sparePart);
    }

    // PUT: api/SparePart/5
    [HttpPut("{id}")]
    public IActionResult UpdateSparePart(int id, SparePartModel sparePart)
    {
        if (id != sparePart.SparePartId)
            return BadRequest();

        _context.Entry(sparePart).State = EntityState.Modified;
        _context.SaveChanges();

        return NoContent();
    }

    // DELETE: api/SparePart/5
    [HttpDelete("{id}")]
    public IActionResult DeleteSparePart(int id)
    {
        var sparePart = _context.SpareParts.Find(id);

        if (sparePart == null)
            return NotFound();

        _context.SpareParts.Remove(sparePart);
        _context.SaveChanges();

        return NoContent();
    }
}