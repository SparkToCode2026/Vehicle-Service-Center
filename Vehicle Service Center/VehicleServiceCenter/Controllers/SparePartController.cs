using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VehicleServiceCenter.Data;
using VehicleServiceCenter.Models;

namespace VehicleServiceCenter.Controllers;
[Route("api/[controller]")]
[ApiController]
public class SparePartController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public SparePartController(ApplicationDbContext context)
    {
        _context = context;
    }


    // GET: api/SparePart
    [HttpGet]
    public async Task<ActionResult<IEnumerable<SparePartModel>>> GetSpareParts()
    {
        var spareParts = await _context.SpareParts
            .Include(s => s.Branch)
            .ToListAsync();

        return Ok(spareParts);
    }


    // GET: api/SparePart/5
    [HttpGet("{id}")]
    public async Task<ActionResult<SparePartModel>> GetSparePart(int id)
    {
        var sparePart = await _context.SpareParts
            .Include(s => s.Branch)
            .FirstOrDefaultAsync(s => s.SparePartId == id);

        if (sparePart == null)
        {
            return NotFound();
        }

        return Ok(sparePart);
    }


    // POST: api/SparePart
    [HttpPost]
    public async Task<ActionResult<SparePartModel>> CreateSparePart(SparePartModel sparePartModel)
    {
        _context.SpareParts.Add(sparePartModel);

        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetSparePart),
            new { id = sparePartModel.SparePartId },
            sparePartModel
        );
    }


    // PUT: api/SparePart/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSparePart(int id, SparePartModel sparePartModel)
    {
        if (id != sparePartModel.SparePartId)
        {
            return BadRequest();
        }

        _context.Entry(sparePartModel).State = EntityState.Modified;

        await _context.SaveChangesAsync();

        return NoContent();
    }
    // DELETE: api/SparePart/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSparePart(int id)
    {
        var sparePart = await _context.SpareParts
            .FindAsync(id);

        if (sparePart == null)
        {
            return NotFound();
        }

        _context.SpareParts.Remove(sparePart);

        await _context.SaveChangesAsync();

        return NoContent();
    }
}