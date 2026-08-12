using Microsoft.AspNetCore.Mvc;
using VehicleServiceCenter.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using VehicleServiceCenter.Services;

namespace VehicleServiceCenter.Controllers
{
    [Authorize]
    [ApiController]
    [Route("Payment")]
    public class PaymentController : ControllerBase
    {
        private ProjectContext context;
        private readonly IResourceAuthorizationService resourceAccess;

        public PaymentController(
            ProjectContext context,
            IResourceAuthorizationService resourceAccess)
        {
            this.context = context;
            this.resourceAccess = resourceAccess;
        }

        // Add payment
        [Authorize(Roles = "Admin,Customer")]
        [HttpPost("AddPayment")]
        public IActionResult AddPayment(PaymentModel payment)
        {
            InvoiceModel? invoice = context.Invoices
                .FirstOrDefault(i =>
                    i.InvoiceId == payment.InvoiceId
                );

            if (invoice == null)
            {
                return BadRequest("Invoice does not exist");
            }

            if (!resourceAccess.IsAdmin &&
                !resourceAccess.CanAccessInvoice(payment.InvoiceId))
            {
                return Forbid();
            }

            if (payment.Amount <= 0)
            {
                return BadRequest(
                    "Payment amount must be greater than zero"
                );
            }

            if (!string.IsNullOrEmpty(payment.TransactionReference))
            {
                PaymentModel? existingPayment =
                    context.Payments.FirstOrDefault(p =>
                        p.TransactionReference ==
                        payment.TransactionReference
                    );

                if (existingPayment != null)
                {
                    return BadRequest(
                        "Transaction reference already exists"
                    );
                }
            }

            payment.PaymentDate = DateTime.Now;

            context.Payments.Add(payment);
            context.SaveChanges();

            return Ok(new
            {
                Message = "Payment added successfully",
                PaymentId = payment.PaymentId
            });
        }

        // Get all payments
        [HttpGet("GetAll")]
        public IActionResult GetAllPayments()
        {
            var payments = resourceAccess.ScopePayments(context.Payments)
                .Include(p => p.Invoice)
                .Select(p => new
                {
                    p.PaymentId,
                    p.InvoiceId,
                    p.Amount,
                    p.PaymentDate,
                    p.PaymentMethod,
                    p.TransactionReference,
                    p.Status,
                    p.Notes,
                    InvoiceNumber = p.Invoice != null
                        ? p.Invoice.InvoiceNumber
                        : null
                })
                .ToList();

            return Ok(payments);
        }

        // Get payment by ID
        [HttpGet("GetById/{id}")]
        public IActionResult GetPaymentById(int id)
        {
            var payment = resourceAccess.ScopePayments(context.Payments)
                .Where(p => p.PaymentId == id)
                .Select(p => new
                {
                    p.PaymentId,
                    p.InvoiceId,
                    p.Amount,
                    p.PaymentDate,
                    p.PaymentMethod,
                    p.TransactionReference,
                    p.Status,
                    p.Notes
                })
                .FirstOrDefault();

            if (payment == null)
            {
                return NotFound("Payment not found");
            }

            return Ok(payment);
        }

        // Update payment by ID
        [Authorize(Roles = "Admin")]
        [HttpPut("Update/{id}")]
        public IActionResult UpdatePayment(
            int id,
            PaymentModel updatedPayment
        )
        {
            PaymentModel? payment =
                context.Payments.Find(id);

            if (payment == null)
            {
                return NotFound("Payment not found");
            }

            InvoiceModel? invoice = context.Invoices
                .FirstOrDefault(i =>
                    i.InvoiceId == updatedPayment.InvoiceId
                );

            if (invoice == null)
            {
                return BadRequest("Invoice does not exist");
            }

            if (updatedPayment.Amount <= 0)
            {
                return BadRequest(
                    "Payment amount must be greater than zero"
                );
            }

            if (!string.IsNullOrEmpty(
                    updatedPayment.TransactionReference
                ))
            {
                PaymentModel? existingTransaction =
                    context.Payments.FirstOrDefault(p =>
                        p.TransactionReference ==
                            updatedPayment.TransactionReference &&
                        p.PaymentId != id
                    );

                if (existingTransaction != null)
                {
                    return BadRequest(
                        "Transaction reference already exists"
                    );
                }
            }

            payment.InvoiceId = updatedPayment.InvoiceId;
            payment.Amount = updatedPayment.Amount;
            payment.PaymentDate = updatedPayment.PaymentDate;
            payment.PaymentMethod = updatedPayment.PaymentMethod;
            payment.TransactionReference =
                updatedPayment.TransactionReference;
            payment.Status = updatedPayment.Status;
            payment.Notes = updatedPayment.Notes;

            context.SaveChanges();

            return Ok(new
            {
                Message = "Payment updated successfully",
                PaymentId = payment.PaymentId
            });
        }
        
        // Filter
        [HttpGet("Filter")]
        public IActionResult FilterPayments(
            string? status,
            string? paymentMethod,
            DateTime? fromDate,
            DateTime? toDate
        )
        {
            if (string.IsNullOrWhiteSpace(status) &&
                string.IsNullOrWhiteSpace(paymentMethod) &&
                !fromDate.HasValue &&
                !toDate.HasValue)
            {
                return BadRequest(
                    "Provide a status, payment method, or date range"
                );
            }

            IQueryable<PaymentModel> query =
                resourceAccess.ScopePayments(context.Payments);

            if (!string.IsNullOrWhiteSpace(status))
            {
                query = query.Where(p => p.Status == status);
            }

            if (!string.IsNullOrWhiteSpace(paymentMethod))
            {
                query = query.Where(p => p.PaymentMethod == paymentMethod);
            }

            if (fromDate.HasValue)
            {
                query = query.Where(p => p.PaymentDate >= fromDate.Value);
            }

            if (toDate.HasValue)
            {
                query = query.Where(p => p.PaymentDate <= toDate.Value);
            }

            var payments = query
                .Select(p => new
                {
                    p.PaymentId,
                    p.InvoiceId,
                    p.Amount,
                    p.PaymentDate,
                    p.PaymentMethod,
                    p.Status
                })
                .ToList();

            return Ok(payments);
        }
        
        // Sort by date 
        [HttpGet("SortByDate")]
        public IActionResult SortPaymentsByDate(bool descending = true)
        {
            IQueryable<PaymentModel> query =
                resourceAccess.ScopePayments(context.Payments);

            query = descending
                ? query.OrderByDescending(p => p.PaymentDate)
                : query.OrderBy(p => p.PaymentDate);

            var payments = query
                .Select(p => new
                {
                    p.PaymentId,
                    p.InvoiceId,
                    p.Amount,
                    p.PaymentDate,
                    p.Status
                })
                .ToList();

            return Ok(payments);
        }
        
        // Get by invoiceID
        [HttpGet("GetTotalByInvoice/{invoiceId}")]
        public IActionResult GetTotalPaidForInvoice(int invoiceId)
        {
            bool invoiceExists = context.Invoices
                .Any(i => i.InvoiceId == invoiceId);

            if (!invoiceExists)
            {
                return NotFound("Invoice not found");
            }

            if (!resourceAccess.CanAccessInvoice(invoiceId))
            {
                return Forbid();
            }

            decimal totalPaid = context.Payments
                .Where(p => p.InvoiceId == invoiceId)
                .Sum(p => (decimal?)p.Amount) ?? 0;

            return Ok(new
            {
                InvoiceId = invoiceId,
                TotalPaid = totalPaid
            });
        }

        // Change payment status
        [Authorize(Roles = "Admin")]
        [HttpPatch("ChangeStatus/{id}")]
        public IActionResult ChangePaymentStatus(
            int id,
            string status
        )
        {
            PaymentModel? payment =
                context.Payments.Find(id);

            if (payment == null)
            {
                return NotFound("Payment not found");
            }

            payment.Status = status;
            context.SaveChanges();

            return Ok(new
            {
                Message = "Payment status changed successfully",
                PaymentId = payment.PaymentId,
                Status = payment.Status
            });
        }

        // Delete payment by ID
        [Authorize(Roles = "Admin")]
        [HttpDelete("Delete/{id}")]
        public IActionResult DeletePayment(int id)
        {
            PaymentModel? payment =
                context.Payments.Find(id);

            if (payment == null)
            {
                return NotFound("Payment not found");
            }

            context.Payments.Remove(payment);
            context.SaveChanges();

            return Ok(new
            {
                Message = "Payment deleted successfully",
                PaymentId = payment.PaymentId
            });
        }
    }
}
