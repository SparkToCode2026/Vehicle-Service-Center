using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VehicleServiceCenter.Models;

public class Appointment
{
    [Key] public int AppointmentId { get; set; }

    public int CustomerProfileId { get; set; }

    [ForeignKey("CustomerProfile")] public CustomerProfile? CustomerProfile { get; set; }

public int VehicleId { get; set; }

    [ForeignKey("Vehicle")]
    public Vehicle Vehicle { get; set; }

    public int ServiceTypeId { get; set; }

    [ForeignKey("ServiceType")]
    public ServiceType ServiceType { get; set; }

    public int MechanicProfileId { get; set; }

    [ForeignKey("MechanicProfile")]
    public MechanicProfile MechanicProfile { get; set; }

    public int BranchId { get; set; }

    [ForeignKey("Branch")]
    public Branch Branch { get; set; }

    public DateTime AppointmentDate { get; set; }

    public string Status { get; set; }

    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; }
}