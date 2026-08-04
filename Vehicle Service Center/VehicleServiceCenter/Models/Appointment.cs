namespace VehicleServiceCenter.Models;

public class Appointment
{
    
    public int AppointmentId { get; set; }

    public DateTime AppointmentDate { get; set; }

    public string Status { get; set; }
    // Pending, Confirmed, Completed, Cancelled

    public string Description { get; set; }

}