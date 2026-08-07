using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace VehicleServiceCenter.Models;

public class AppointmentModel
{
    [Key]
    public int AppointmentId { get; set; }

    public int CustomerProfileId { get; set; }
    public CustomerProfileModel CustomerProfile { get; set; } = null!;

    public int VehicleId { get; set; }
    public VehicleModel Vehicle { get; set; } = null!;

    public int ServiceTypeId { get; set; }
    public ServiceTypeModel ServiceType { get; set; } = null!;

    public int? MechanicProfileId { get; set; }
    public MechanicProfileModel? MechanicProfile { get; set; }

    public int BranchId { get; set; }
    public BranchModel Branch { get; set; } = null!;

    public DateTime AppointmentDate { get; set; }

    [Required, MaxLength(30)]
    public string Status { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; }

    [JsonIgnore]
    public ServiceOrderModel? ServiceOrder { get; set; }
}
