namespace VehicleServiceCenter.Models
{
    public class ServiceTypeModel
    {
        public int ServiceTypeId { get; set; }
        public string SName { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public double SPrice { get; set; }
    }
}
