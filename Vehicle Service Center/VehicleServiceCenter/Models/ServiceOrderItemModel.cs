using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VehicleServiceCenter.Models
{
    public class ServiceOrderItemModel
    {
        [Key]
        public int ServiceOrderItemId { get; set; }

        [Required]
        public int ServiceOrderId { get; set; }
        public ServiceOrderModel? ServiceOrder { get; set; }

       
        public int? ServiceTypeId { get; set; }
        public ServiceTypeModel? ServiceType { get; set; }

        public int? SparePartId { get; set; }
        public SparePart? SparePart { get; set; }

        [Required]
        [MaxLength(20)]
        public string ItemType { get; set; } = "Service"; 

        [MaxLength(500)]
        public string? Description { get; set; }

        [Required]
        public int Quantity { get; set; } = 1;

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal UnitPrice { get; set; }

        [Column(TypeName = "decimal(6,2)")]
        public decimal? LaborHours { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Subtotal { get; set; }
    }
}