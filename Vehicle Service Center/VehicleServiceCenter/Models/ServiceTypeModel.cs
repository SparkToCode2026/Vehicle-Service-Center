using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace VehicleServiceCenter.Models
{
    [Index(nameof(Name), IsUnique = true)]
    public class ServiceTypeModel
    {
        [Key]
        public int ServiceTypeId { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal BasePrice { get; set; }

        public int EstimatedDurationMinutes { get; set; }

        public bool IsActive { get; set; } = true;

        [JsonIgnore]
        public List<AppointmentModel> Appointments { get; set; } = new();

        [JsonIgnore]
        public List<ServiceOrderItemModel> ServiceOrderItems { get; set; } = new();
    }
}
