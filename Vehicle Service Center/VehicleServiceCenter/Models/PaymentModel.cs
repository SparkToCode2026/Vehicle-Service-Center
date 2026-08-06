using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VehicleServiceCenter.Models;

public class PaymentModel
{
    [Key] public int PaymentId { get; set; }

    public int InvoiceId { get; set; }

    [ForeignKey("Invoice")]
    public InvoiceModel? Invoice { get; set; }

    [Required, Column(TypeName = "decimal(10,2)")]
    public decimal Amount { get; set; }

    [Required]
    public DateTime PaymentDate { get; set; }

    [Required, MaxLength(30)]
    public string PaymentMethod { get; set; }

    [MaxLength(100)]
    public string? TransactionReference { get; set; }

    [Required, MaxLength(30)]
    public string Status { get; set; }

    [MaxLength(500)]
    public string? Notes { get; set; }
}