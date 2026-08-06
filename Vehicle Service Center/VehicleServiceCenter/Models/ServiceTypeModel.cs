using System.ComponentModel.DataAnnotations;

namespace VehicleServiceCenter.Models
{
    public class ServiceTypeModel
    {
        [Key]
        public int ServiceTypeId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double BasePrice { get; set; }
        public int EstimatedDurationMinutes { get; set; } 
        public bool IsActive { get; set; } = true;



        /*One to Many relationship with Appointment
        public List<Appointment> Appointments { get; set; } 

        in the Appointment class, you would have a foreign key property:
        [ForeignKey("ServiceType")]
        public int ServiceTypeId { get; set; } //foreign key to ServiceTypeModel
        public ServiceTypeModel ServiceType { get; set; } //navigation property to ServiceTypeModel
        */

        /*One to Many relationship with ServiceOrderItem
        public List<ServiceOrderItem> ServiceOrderItems { get; set; } 

        in the ServiceOrderItem class, you would have a foreign key property:
        [ForeignKey("ServiceType")]
        public int ServiceTypeId { get; set; } //foreign key to ServiceTypeModel
        public ServiceTypeModel ServiceType { get; set; } //navigation property to ServiceTypeModel
        */
    }
}
