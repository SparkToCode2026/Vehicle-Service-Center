using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace VehicleServiceCenter.Models;

[Index(nameof(PartNumber), IsUnique = true)]
public class SparePartModel
{
    [Key]
    public int SparePartId { get; set; }

    public int BranchId { get; set; }
    public BranchModel Branch { get; set; } = null!;

    [Required, MaxLength(100)]
    public string PartName { get; set; } = string.Empty;

    [Required, MaxLength(50)]
    public string PartNumber { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal UnitPrice { get; set; }

    public int StockQuantity { get; set; }

    public int ReorderLevel { get; set; }

    public bool IsAvailable { get; set; }

    [JsonIgnore]
    public List<ServiceOrderItemModel> ServiceOrderItems { get; set; } = new();
}
