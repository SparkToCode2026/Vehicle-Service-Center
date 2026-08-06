using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VehicleServiceCenter.Models;

namespace VehicleServiceCenter.Controllers;
{
    [ApiController]
    [Route("Payment")]
    public class PaymentController : ControllerBase
    {
        private ProjectContext ProjectContext;

        public PaymentController(ProjectContext projectContext)
        {
            ProjectContext = projectContext;
        }

        // Add payment
        [HttpPost("AddPayment")]
        public IActionResult AddPayment(PaymentModel payment)
        {
            // Check whether the invoice exists
            InvoiceModel invoice = ProjectContext.Invoices
                .FirstOrDefault(i => i.InvoiceId == payment.InvoiceId);

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
        }

    }
    // Check whether transaction reference already exists
    if (!string.IsNullOrEmpty(payment.TransactionReference))
    {
        PaymentModel existingPayment =
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
    
    

