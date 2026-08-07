using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace VehicleServiceCenter.Models;

public class BranchModel
{
    [Key]
    public int BranchId { get; set; }

    [Required, MaxLength(100)]
    public string BranchName { get; set; } = string.Empty;

    [Required, MaxLength(255)]
    public string Address { get; set; } = string.Empty;

    [Required, MaxLength(20)]
    public string PhoneNumber { get; set; } = string.Empty;

    [MaxLength(150)]
    public string? Email { get; set; }

    public TimeSpan OpeningTime { get; set; }

    public TimeSpan ClosingTime { get; set; }

    public bool IsActive { get; set; }

    [JsonIgnore]
    public List<MechanicProfileModel> MechanicProfiles { get; set; } = new();

    [JsonIgnore]
    public List<ServiceOrderModel> ServiceOrders { get; set; } = new();

    [JsonIgnore]
    public List<AppointmentModel> Appointments { get; set; } = new();

    [JsonIgnore]
    public List<SparePartModel> SpareParts { get; set; } = new();
}
