using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace VehicleServiceCenter.Models
{
    public class MechanicProfileModel
    {
        [Key]
        public int MechanicProfileId { get; set; }

        [Required]
        public string Specialization { get; set; } = string.Empty;

        public int ExperienceYears { get; set; }

        public DateOnly HireDate { get; set; }

        public bool IsAvailable { get; set; } = true;

        // One-to-one relationship with User
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }

        [JsonIgnore]
        public UserModel? User { get; set; }

        // One Branch has many mechanics
        [ForeignKey(nameof(Branch))]
        public int BranchId { get; set; }

        [JsonIgnore]
        public BranchModel? Branch { get; set; }
    }
}