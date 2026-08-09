using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace VehicleServiceCenter.Models
{
    [Index(nameof(AppointmentId), IsUnique = true)]
    public class ServiceOrderModel
    {
        [Key]
        public int ServiceOrderId { get; set; }

        public int? AppointmentId { get; set; }
        public AppointmentModel? Appointment { get; set; }

        public int CustomerProfileId { get; set; }
        public CustomerProfileModel CustomerProfile { get; set; } = null!;

        public int VehicleId { get; set; }
        public VehicleModel Vehicle { get; set; } = null!;

        public int? MechanicProfileId { get; set; }
        public MechanicProfileModel? MechanicProfile { get; set; }

        public int BranchId { get; set; }
        public BranchModel Branch { get; set; } = null!;

        public DateTime OrderDate { get; set; }

        public DateTime? CompletionDate { get; set; }

        [Required, MaxLength(30)]
        public string Status { get; set; } = "Pending";

        [MaxLength(500)]
        public string? CustomerComplaint { get; set; }

        [MaxLength(500)]
        public string? Diagnosis { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal TotalAmount { get; set; }

        public DateTime CreatedAt { get; set; }

        public List<ServiceOrderItemModel> ServiceOrderItems { get; set; } = new();

        [JsonIgnore]
        public InvoiceModel? Invoice { get; set; }
    }
}
