using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace VehicleServiceCenter.Models
{
    [Index(nameof(UserId), IsUnique = true)]
    public class MechanicProfileModel
    {
        [Key]
        public int MechanicProfileId { get; set; }

        public int UserId { get; set; }

        public int BranchId { get; set; }

        [Required, MaxLength(100)]
        public string Specialization { get; set; } = string.Empty;

        public int ExperienceYears { get; set; }

        public DateOnly HireDate { get; set; }

        public bool IsAvailable { get; set; } = true;

        [JsonIgnore]
        public UserModel? User { get; set; }

        [JsonIgnore]
        public BranchModel? Branch { get; set; }

        [JsonIgnore]
        public List<AppointmentModel> Appointments { get; set; } = new();

        [JsonIgnore]
        public List<ServiceOrderModel> ServiceOrders { get; set; } = new();
    }
}
