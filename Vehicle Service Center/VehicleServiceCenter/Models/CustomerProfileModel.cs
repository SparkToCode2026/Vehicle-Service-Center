using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace VehicleServiceCenter.Models
{
    [Index(nameof(UserId), IsUnique = true)]
    public class CustomerProfileModel
    {
        [Key]
        public int CustomerProfileId { get; set; }

        public int UserId { get; set; }

        [MaxLength(255)]
        public string? Address { get; set; }

        public DateOnly? DateOfBirth { get; set; }

        public DateTime CreatedAt { get; set; }

        [JsonIgnore]
        public UserModel? User { get; set; }

        [JsonIgnore]
        public List<VehicleModel> Vehicles { get; set; } = new();

        [JsonIgnore]
        public List<AppointmentModel> Appointments { get; set; } = new();

        [JsonIgnore]
        public List<ServiceOrderModel> ServiceOrders { get; set; } = new();
    }
}
