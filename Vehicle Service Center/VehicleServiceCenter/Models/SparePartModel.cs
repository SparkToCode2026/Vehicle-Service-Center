using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VehicleServiceCenter.Models;


public class SparePartModel
{
    [Key]
    public int SparePartId { get; set; }

    [ForeignKey("Branch")]
    public int BranchId { get; set; }
    public BranchModel Branch { get; set; }

    public string PartName { get; set; }

    public string PartNumber { get; set; }

    public string? Description { get; set; }

    public decimal UnitPrice { get; set; }

    public int StockQuantity { get; set; }

    public int ReorderLevel { get; set; }

    public bool IsAvailable { get; set; }

    public List<ServiceOrderItemModel> ServiceOrderItems { get; set; }
}