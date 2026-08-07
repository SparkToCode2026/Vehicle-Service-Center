using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VehicleServiceCenter.Models;

public class AppointmentModel
{
    [Key] public int AppointmentId { get; set; }

    public int CustomerProfileId { get; set; }

    [ForeignKey("CustomerProfile")] public CustomerProfileModel? CustomerProfile { get; set; }

public int VehicleId { get; set; }

    [ForeignKey("Vehicle")]
    public VehicleModel Vehicle { get; set; }

    public int ServiceTypeId { get; set; }

    [ForeignKey("ServiceType")]
    public ServiceTypeModel ServiceType { get; set; }

    public int MechanicProfileId { get; set; }

    [ForeignKey("MechanicProfile")]
    public MechanicProfileModel MechanicProfile { get; set; }

    

    [ForeignKey("Branch")] 
    public int BranchId { get; set; }
    public BranchModel Branch { get; set; }

    public DateTime AppointmentDate { get; set; }

    public string Status { get; set; }

    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; }
}