using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace VehicleServiceCenter.Models;

[Index(nameof(ServiceOrderId), IsUnique = true)]
[Index(nameof(InvoiceNumber), IsUnique = true)]
public class InvoiceModel
{
    [Key]
    public int InvoiceId { get; set; }

    public int ServiceOrderId { get; set; }

    [JsonIgnore]
    public ServiceOrderModel ServiceOrder { get; set; } = null!;

    [Required, MaxLength(50)]
    public string InvoiceNumber { get; set; } = string.Empty;

    public DateTime IssueDate { get; set; }

    public DateTime? DueDate { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal Subtotal { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal TaxAmount { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal DiscountAmount { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal TotalAmount { get; set; }

    [Required, MaxLength(30)]
    public string Status { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Notes { get; set; }

    public List<PaymentModel> Payments { get; set; } = new();
}
