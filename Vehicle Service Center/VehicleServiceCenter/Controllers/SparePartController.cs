using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VehicleServiceCenter.Models;

namespace VehicleServiceCenter.Controllers;

[ApiController]
[Route("[controller]")]
public class SparePartController : ControllerBase
{
    private readonly ProjectContext context;

    public SparePartController(ProjectContext context)
    {
        this.context = context;
    }

    // 1. GET - Get all spare parts with Branch
    [HttpGet]
    public async Task<IActionResult> GetSpareParts()
    {
        var spareParts = await context.SpareParts
            .Include(s => s.Branch)
            .ToListAsync();

        return Ok(spareParts);
    }

    // 2. GET - Get spare part by ID
    [HttpGet("{id}")]
    public async Task<IActionResult> GetSparePart(int id)
    {
        var sparePart = await context.SpareParts
            .Include(s => s.Branch)
            .FirstOrDefaultAsync(s => s.SparePartId == id);

        if (sparePart == null)
            return NotFound();

        return Ok(sparePart);
    }

    // 3. POST - Create spare part
    [HttpPost]
    public async Task<ActionResult<SparePartModel>> CreateSparePart(
        SparePartModel sparePart)
    {
        context.SpareParts.Add(sparePart);

        await context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetSparePart),
            new { id = sparePart.SparePartId },
            sparePart);
    }

    // 4. PUT - Update spare part
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSparePart(
        int id,
        SparePartModel sparePart)
    {
        var existingSparePart = await context.SpareParts.FindAsync(id);

        if (existingSparePart == null)
            return NotFound();

        existingSparePart.BranchId = sparePart.BranchId;
        existingSparePart.PartName = sparePart.PartName;
        existingSparePart.PartNumber = sparePart.PartNumber;
        existingSparePart.Description = sparePart.Description;
        existingSparePart.UnitPrice = sparePart.UnitPrice;
        existingSparePart.StockQuantity = sparePart.StockQuantity;
        existingSparePart.ReorderLevel = sparePart.ReorderLevel;
        existingSparePart.IsAvailable = sparePart.IsAvailable;

        await context.SaveChangesAsync();

        return NoContent();
    }

    // 5. DELETE - Delete spare part
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSparePart(int id)
    {
        var sparePart = await context.SpareParts.FindAsync(id);

        if (sparePart == null)
            return NotFound();

        context.SpareParts.Remove(sparePart);

        await context.SaveChangesAsync();

        return NoContent();
    }

    // 6. GET - Filter spare parts by availability
    [HttpGet("filter/available")]
    public async Task<IActionResult> GetAvailableSpareParts()
    {
        var spareParts = await context.SpareParts
            .Include(s => s.Branch)
            .Where(s => s.IsAvailable && s.StockQuantity > 0)
            .ToListAsync();

        return Ok(spareParts);
    }

    // 7. GET - Sort spare parts by price
    [HttpGet("sorted")]
    public async Task<IActionResult> GetSparePartsSortedByPrice()
    {
        var spareParts = await context.SpareParts
            .Include(s => s.Branch)
            .OrderBy(s => s.UnitPrice)
            .ToListAsync();

        return Ok(spareParts);
    }

    // 8. PATCH - Change stock quantity
    [HttpPatch("{id}/stock")]
    public async Task<IActionResult> UpdateStockQuantity(
        int id,
        [FromBody] int quantity)
    {
        var sparePart = await context.SpareParts.FindAsync(id);

        if (sparePart == null)
            return NotFound();

        if (quantity < 0)
            return BadRequest("Stock quantity cannot be negative.");

        sparePart.StockQuantity = quantity;

        sparePart.IsAvailable = quantity > 0;

        await context.SaveChangesAsync();

        return Ok(sparePart);
    }
}