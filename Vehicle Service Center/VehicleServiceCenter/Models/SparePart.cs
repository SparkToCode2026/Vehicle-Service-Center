using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VehicleServiceCenter.Models;

public class SparePart
{
    [Key]
    public int SparePartId { get; set; }

    public int BranchId { get; set; }

    [ForeignKey("Branch")]
    public Branch Branch { get; set; }

    public string PartName { get; set; }

    public string PartNumber { get; set; }

    public string? Description { get; set; }

    public decimal UnitPrice { get; set; }

    public int StockQuantity { get; set; }

    public int ReorderLevel { get; set; }

    public bool IsAvailable { get; set; }

    public List<ServiceOrderItem> ServiceOrderItems { get; set; }
}