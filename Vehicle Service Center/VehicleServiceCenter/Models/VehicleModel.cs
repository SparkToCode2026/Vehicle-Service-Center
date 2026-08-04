namespace VehicleServiceCenter.Models
{
    public class VehicleModel
    {
        public int VehicleId { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }

        /*One to Many relationship with Appointment
        public List<Appointment> Appointments { get; set; } 

        in the Appointment class, you would have a foreign key property:
        [ForeignKey("Vehicle")]
        public int VehicleId { get; set; } //foreign key to VehicleModel
        public VehicleModel Vehicle { get; set; } //navigation property to VehicleModel
        */

        /*One to Many relationship with ServiceOrder
        public List<ServiceOrder> ServiceOrders { get; set; } 

        in the ServiceOrder class, you would have a foreign key property:
        [ForeignKey("Vehicle")]
        public int VehicleId { get; set; } //foreign key to VehicleModel
        public VehicleModel Vehicle { get; set; } //navigation property to VehicleModel
        */
    }
}
