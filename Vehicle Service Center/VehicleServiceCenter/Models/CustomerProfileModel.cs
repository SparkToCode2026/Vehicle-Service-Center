using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace VehicleServiceCenter.Models
{
    public class CustomerProfileModel
    {
        [Key]
        public int CustomerProfileId { get; set; }
        [Required]
        public string Address { get; set; } = string.Empty;

        public DateOnly DateOfBirth { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [ForeignKey(nameof(User))]
        public int UserId { get; set; }

        [JsonIgnore]
        public UserModel? User { get; set; }
    }
}