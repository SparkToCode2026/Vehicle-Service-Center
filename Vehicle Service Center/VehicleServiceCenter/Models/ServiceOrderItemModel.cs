using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace VehicleServiceCenter.Models
{
    public class ServiceOrderItemModel
    {
        [Key]
        public int ServiceOrderItemId { get; set; }

        public int ServiceOrderId { get; set; }

        [JsonIgnore]
        public ServiceOrderModel ServiceOrder { get; set; } = null!;

        public int? ServiceTypeId { get; set; }
        public ServiceTypeModel? ServiceType { get; set; }

        public int? SparePartId { get; set; }
        public SparePartModel? SparePart { get; set; }

        [Required, MaxLength(20)]
        public string ItemType { get; set; } = "Service";

        [MaxLength(500)]
        public string? Description { get; set; }

        public int Quantity { get; set; } = 1;

        [Column(TypeName = "decimal(10,2)")]
        public decimal UnitPrice { get; set; }

        [Column(TypeName = "decimal(6,2)")]
        public decimal? LaborHours { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Subtotal { get; set; }
    }
}
