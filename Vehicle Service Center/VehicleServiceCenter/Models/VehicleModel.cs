using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace VehicleServiceCenter.Models
{
    [Index(nameof(PlateNumber), IsUnique = true)]
    [Index(nameof(VIN), IsUnique = true)]
    public class VehicleModel
    {
        [Key]
        public int VehicleId { get; set; }

        public int CustomerProfileId { get; set; }

        [Required, MaxLength(30)]
        public string PlateNumber { get; set; } = string.Empty;

        [MaxLength(50)]
        public string? VIN { get; set; }

        [Required, MaxLength(50)]
        public string Make { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string Model { get; set; } = string.Empty;

        public int Year { get; set; }

        [MaxLength(30)]
        public string? Color { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal? Mileage { get; set; }

        public DateTime CreatedAt { get; set; }

        public CustomerProfileModel CustomerProfile { get; set; } = null!;

        [JsonIgnore]
        public List<AppointmentModel> Appointments { get; set; } = new();

        [JsonIgnore]
        public List<ServiceOrderModel> ServiceOrders { get; set; } = new();
    }
}
