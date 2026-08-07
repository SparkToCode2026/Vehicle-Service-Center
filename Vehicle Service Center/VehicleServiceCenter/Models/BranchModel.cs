using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VehicleServiceCenter.Models;

public class BranchModel
{
    [Key] public int BranchId { get; set; }

    [Required, MaxLength(100)]
    public string BranchName { get; set; }

    [Required, MaxLength(255)]
    public string Address { get; set; }

    [Required, MaxLength(20)]
    public string PhoneNumber { get; set; }

    [MaxLength(150)]
    public string? Email { get; set; }

    [Required]
    public TimeSpan OpeningTime { get; set; }

    [Required]
    public TimeSpan ClosingTime { get; set; }

    [Required]
    public bool IsActive { get; set; }

    public ICollection<MechanicProfileModel>? MechanicProfiles { get; set; }
    public ICollection<ServiceOrderModel>? ServiceOrders { get; set; }
    public ICollection<AppointmentModel>? Appointments { get; set; }
    public ICollection<SparePartModel>? SpareParts { get; set; }
}