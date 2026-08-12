using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using VehicleServiceCenter.Models;

namespace VehicleServiceCenter.Controllers;

[Authorize(Roles = "Admin,Mechanic")]
[ApiController]
[Route("[controller]")]
public class SparePartController : ControllerBase
{
    private readonly ProjectContext context;

    public SparePartController(ProjectContext context)
    {
        this.context = context;
    }

    // 1. POST - Create a new spare part
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public IActionResult CreateSparePart(SparePartModel sparePart)
    {
        context.SpareParts.Add(sparePart);
        context.SaveChanges();

        return CreatedAtAction(
            nameof(GetSparePart),
            new { id = sparePart.SparePartId },
            sparePart
        );
    }

    // 2. PUT - Update spare part details
    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public IActionResult UpdateSparePart(int id, SparePartModel sparePart)
    {
        var existingSparePart = context.SpareParts.Find(id);

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

        context.SaveChanges();

        return NoContent();
    }

    // 3. PATCH - Update spare part stock quantity
    [Authorize(Roles = "Admin,Mechanic")]
    [HttpPatch("{id}/stock")]
    public IActionResult UpdateStockQuantity(int id, int quantity)
    {
        var sparePart = context.SpareParts.Find(id);

        if (sparePart == null)
            return NotFound();

        sparePart.StockQuantity = quantity;

        // Automatically update availability
        sparePart.IsAvailable = quantity > 0;

        context.SaveChanges();

        return Ok(sparePart);
    }

    // 4. DELETE - Delete a spare part
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public IActionResult DeleteSparePart(int id)
    {
        var sparePart = context.SpareParts.Find(id);

        if (sparePart == null)
            return NotFound();

        var isUsed = context.ServiceOrderItems
            .Any(x => x.SparePartId == id);

        if (isUsed)
        {
            return BadRequest(
                "This spare part cannot be deleted because it is used in a service order."
            );
        }

        context.SpareParts.Remove(sparePart);
        context.SaveChanges();

        return NoContent();
    }

    // 5. GET - Get all spare parts with Branch
    [HttpGet]
    public IActionResult GetSpareParts()
    {
        var spareParts = context.SpareParts
            .Include(s => s.Branch)
            .ToList();

        return Ok(spareParts);
    }

    // 6. GET - Get spare part by ID
    [HttpGet("{id}")]
    public IActionResult GetSparePart(int id)
    {
        var sparePart = context.SpareParts
            .Include(s => s.Branch)
            .FirstOrDefault(s => s.SparePartId == id);

        if (sparePart == null)
            return NotFound();

        return Ok(sparePart);
    }

    // 7. GET - Filter spare parts by availability
    [HttpGet("filter")]
    public IActionResult GetAvailableSpareParts(bool isAvailable)
    {
        var spareParts = context.SpareParts
            .Where(s => s.IsAvailable == isAvailable)
            .Include(s => s.Branch)
            .ToList();

        return Ok(spareParts);
    }

    // 8. GET - Sort spare parts by price
    [HttpGet("sort")]
    public IActionResult GetSparePartsSorted()
    {
        var spareParts = context.SpareParts
            .OrderBy(s => s.UnitPrice)
            .Include(s => s.Branch)
            .ToList();

        return Ok(spareParts);
    }
}
