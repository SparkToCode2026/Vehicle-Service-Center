using Microsoft.AspNetCore.Mvc;
using VehicleServiceCenter.Models;

namespace VehicleServiceCenter.Controllers
{
    [ApiController]
    [Route("Payment")]
    public class PaymentController : ControllerBase
    {
        private readonly ProjectContext ProjectContext;

        public PaymentController(ProjectContext projectContext)
        {
            ProjectContext = projectContext;
        }

        // Add payment
        [HttpPost("AddPayment")]
        public IActionResult AddPayment(PaymentModel payment)
        {
            InvoiceModel? invoice = ProjectContext.Invoices
                .FirstOrDefault(i =>
                    i.InvoiceId == payment.InvoiceId
                );

            if (invoice == null)
            {
                return BadRequest("Invoice does not exist");
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
                    ProjectContext.Payments.FirstOrDefault(p =>
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

            ProjectContext.Payments.Add(payment);
            ProjectContext.SaveChanges();

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
            var payments = ProjectContext.Payments
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
                .ToList();

            return Ok(payments);
        }

        // Get payment by ID
        [HttpGet("GetById/{id}")]
        public IActionResult GetPaymentById(int id)
        {
            var payment = ProjectContext.Payments
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
        [HttpPut("Update/{id}")]
        public IActionResult UpdatePayment(
            int id,
            PaymentModel updatedPayment
        )
        {
            PaymentModel? payment =
                ProjectContext.Payments.Find(id);

            if (payment == null)
            {
                return NotFound("Payment not found");
            }

            InvoiceModel? invoice = ProjectContext.Invoices
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
                    ProjectContext.Payments.FirstOrDefault(p =>
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

            ProjectContext.SaveChanges();

            return Ok(new
            {
                Message = "Payment updated successfully",
                PaymentId = payment.PaymentId
            });
        }

        // Change payment status
        [HttpPatch("ChangeStatus/{id}")]
        public IActionResult ChangePaymentStatus(
            int id,
            string status
        )
        {
            PaymentModel? payment =
                ProjectContext.Payments.Find(id);

            if (payment == null)
            {
                return NotFound("Payment not found");
            }

            payment.Status = status;
            ProjectContext.SaveChanges();

            return Ok(new
            {
                Message = "Payment status changed successfully",
                PaymentId = payment.PaymentId,
                Status = payment.Status
            });
        }

        // Delete payment by ID
        [HttpDelete("Delete/{id}")]
        public IActionResult DeletePayment(int id)
        {
            PaymentModel? payment =
                ProjectContext.Payments.Find(id);

            if (payment == null)
            {
                return NotFound("Payment not found");
            }

            ProjectContext.Payments.Remove(payment);
            ProjectContext.SaveChanges();

            return Ok(new
            {
                Message = "Payment deleted successfully",
                PaymentId = payment.PaymentId
            });
        }
    }
}