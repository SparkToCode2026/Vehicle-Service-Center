using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VehicleServiceCenter.Models
{
    [Index(nameof(Email), IsUnique = true)]
    public class UserModel
    {
        [Key]
        public int UserId { get; set; }

        [Required, MaxLength(100)]
        [Column("Name")]
        public string UserName { get; set; } = string.Empty;

        [Required, MaxLength(255)]
        [Column("PasswordHash")]
        public string Password { get; set; } = string.Empty;

        [Required, MaxLength(150)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required, MaxLength(20)]
        public string Role { get; set; } = string.Empty;

        [MaxLength(20)]
        public string? PhoneNumber { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; }

        public CustomerProfileModel? CustomerProfile { get; set; }
        public MechanicProfileModel? MechanicProfile { get; set; }
    }
}
