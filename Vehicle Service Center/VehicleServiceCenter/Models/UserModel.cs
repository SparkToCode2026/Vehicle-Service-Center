using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace VehicleServiceCenter.Models
{
    public class UserModel
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        public string UserName { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Role { get; set; } = string.Empty;

        [Required]
        public string PhoneNumber { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // One-to-one relationships
        public CustomerProfileModel? CustomerProfile { get; set; }

        public MechanicProfileModel? MechanicProfile { get; set; }
    }
}
