using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VehicleServiceCenter.Models
{
    public class ServiceOrderModel
    {
        [Key]
        public int ServiceOrderId { get; set; }

        
        public int? AppointmentId { get; set; }
        public AppointmentModel? Appointment { get; set; }
       
        [Required]
        public int CustomerProfileId { get; set; }

        [Required]
        public int VehicleId { get; set; }
        public VehicleModel? Vehicle { get; set; }

        public int? MechanicProfileId { get; set; }

        [Required]
        public int BranchId { get; set; }

        [Required]
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        public DateTime? CompletionDate { get; set; }

        [Required]
        [MaxLength(30)]
        public string Status { get; set; } = "Pending"; 

        [MaxLength(500)]
        public string? CustomerComplaint { get; set; }

        [MaxLength(500)]
        public string? Diagnosis { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal TotalAmount { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<ServiceOrderItemModel> ServiceOrderItems { get; set; } = new List<ServiceOrderItemModel>();
    }
}