using System.ComponentModel.DataAnnotations;

namespace VehicleServiceCenter.Models
{
    public class VehicleModel
    {
        [Key]
        public int VehicleId { get; set; }
        public string Make { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int Year { get; set; }
        public string VIN { get; set; } = string.Empty;
        public string PlateNumber { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public double Mileage { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

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

        /*Many to One relationship with CustomerProfile
        public int CustomerProfileId { get; set; } //foreign key to CustomerProfile
        public CustomerProfileModel CustomerProfile { get; set; } //navigation property to CustomerProfileModel
        */
    }
}
