namespace VehicleServiceCenter.Models;

public class SparePart
{
    
    public int SparePartId { get; set; }

    public string PartName { get; set; }

    public int Quantity { get; set; }

    public decimal Price { get; set; }

    public string Supplier { get; set; }


    public bool IsAvailable { get; set; }
    
}