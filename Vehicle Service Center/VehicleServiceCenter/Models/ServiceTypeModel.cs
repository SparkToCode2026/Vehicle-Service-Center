namespace VehicleServiceCenter.Models
{
    public class ServiceTypeModel
    {
        public int ServiceTypeId { get; set; }
        public string SName { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public double SPrice { get; set; }

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
