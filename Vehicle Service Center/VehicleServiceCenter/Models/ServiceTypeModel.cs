using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace VehicleServiceCenter.Models
{
    public class ServiceTypeModel
    {
        [Key]
        public int ServiceTypeId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        [Column(TypeName = "decimal(10,2)")]
        public decimal BasePrice { get; set; }
        public int EstimatedDurationMinutes { get; set; } 
        public bool IsActive { get; set; } = true;  

        // One-to-many relationships 
        public List<AppointmentModel> Appointments { get; set; } 
        public List<ServiceOrderItemModel> ServiceOrderItems { get; set; }
    }
}
