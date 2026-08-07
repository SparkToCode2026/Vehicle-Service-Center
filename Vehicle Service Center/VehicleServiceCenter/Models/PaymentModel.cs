using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace VehicleServiceCenter.Models;

[Index(nameof(TransactionReference), IsUnique = true)]
public class PaymentModel
{
    [Key]
    public int PaymentId { get; set; }

    public int InvoiceId { get; set; }

    [JsonIgnore]
    public InvoiceModel Invoice { get; set; } = null!;

    [Column(TypeName = "decimal(10,2)")]
    public decimal Amount { get; set; }

    public DateTime PaymentDate { get; set; }

    [Required, MaxLength(30)]
    public string PaymentMethod { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? TransactionReference { get; set; }

    [Required, MaxLength(30)]
    public string Status { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Notes { get; set; }
}
