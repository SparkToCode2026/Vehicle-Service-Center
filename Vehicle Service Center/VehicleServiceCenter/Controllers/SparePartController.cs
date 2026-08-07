using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VehicleServiceCenter.Models;

namespace VehicleServiceCenter.Controllers;

[ApiController]
[Route("[controller]")]
public class SparePartController : ControllerBase
{
    private ProjectContext context;

    public SparePartController(ProjectContext context)
    {
        context = context;
    }

    // GET: api/SparePart
    [HttpGet]
    public IActionResult GetSpareParts()
    {
        return Ok(context.SpareParts.ToList());
    }

    // GET: api/SparePart/5
    [HttpGet("{id}")]
    public IActionResult GetSparePart(int id)
    {
        var sparePart = context.SpareParts.Find(id);

        if (sparePart == null)
            return NotFound();

        return Ok(sparePart);
    }

    // POST: api/SparePart
    [HttpPost]
    public IActionResult CreateSparePart(SparePartModel sparePart)
    {
        context.SpareParts.Add(sparePart);
        context.SaveChanges();

        return CreatedAtAction(nameof(GetSparePart),
            new { id = sparePart.SparePartId }, sparePart);
    }

    // PUT: api/SparePart/5
    [HttpPut("{id}")]
    public IActionResult UpdateSparePart(int id, SparePartModel sparePart)
    {
        if (id != sparePart.SparePartId)
            return BadRequest();

        context.Entry(sparePart).State = EntityState.Modified;
        context.SaveChanges();

        return NoContent();
    }

    // DELETE: api/SparePart/5
    [HttpDelete("{id}")]
    public IActionResult DeleteSparePart(int id)
    {
        var sparePart = context.SpareParts.Find(id);

        if (sparePart == null)
            return NotFound();

        context.SpareParts.Remove(sparePart);
        context.SaveChanges();

        return NoContent();
    }
}