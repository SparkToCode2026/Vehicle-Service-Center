using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VehicleServiceCenter.Models;

public class InvoiceModel
{
    [Key] public int InvoiceId { get; set; }

    public int ServiceOrderId { get; set; }

    [ForeignKey("ServiceOrder")]
    public ServiceOrderModel? ServiceOrder { get; set; }

    [Required, MaxLength(50)]
    public string InvoiceNumber { get; set; }

    [Required]
    public DateTime IssueDate { get; set; }

    public DateTime? DueDate { get; set; }

    [Required, Column(TypeName = "decimal(10,2)")]
    public decimal Subtotal { get; set; }

    [Required, Column(TypeName = "decimal(10,2)")]
    public decimal TaxAmount { get; set; }

    [Required, Column(TypeName = "decimal(10,2)")]
    public decimal DiscountAmount { get; set; }

    [Required, Column(TypeName = "decimal(10,2)")]
    public decimal TotalAmount { get; set; }

    [Required, MaxLength(30)]
    public string Status { get; set; }

    [MaxLength(500)]
    public string? Notes { get; set; }

    public ICollection<PaymentModel> Payments { get; set; } = new List<PaymentModel>();
}