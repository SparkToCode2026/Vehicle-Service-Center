using Microsoft.AspNetCore.Mvc;
using VehicleServiceCenter.Models;

namespace VehicleServiceCenter.Controllers
{
    [ApiController]
    [Route("Invoice")]
    public class InvoiceController : ControllerBase
    {
        private ProjectContext ProjectContext;

        public InvoiceController(ProjectContext projectContext)
        {
            ProjectContext = projectContext;
        }

        // Add invoice
        [HttpPost("AddInvoice")]
        public IActionResult AddInvoice(InvoiceModel invoice)
        {
            // Check whether the service order exists
            ServiceOrderModel serviceOrder =
                ProjectContext.ServiceOrders.FirstOrDefault(s =>
                    s.ServiceOrderId == invoice.ServiceOrderId
                );

            if (serviceOrder == null)
            {
                return BadRequest(
                    "Service order does not exist"
                );
            }
            // Check whether invoice number already exists
            InvoiceModel existingInvoice =
                ProjectContext.Invoices.FirstOrDefault(i =>
                    i.InvoiceNumber == invoice.InvoiceNumber
                );

            if (existingInvoice != null)
            {
                return BadRequest(
                    "Invoice number already exists"
                );
            }

            if (invoice.Subtotal < 0)
            {
                return BadRequest(
                    "Subtotal cannot be negative"
                );
            }

            if (invoice.TaxAmount < 0)
            {
                return BadRequest(
                    "Tax amount cannot be negative"
                );
            }

            if (invoice.DiscountAmount < 0)
            {
                return BadRequest(
                    "Discount amount cannot be negative"
                );
            }

            invoice.TotalAmount =
                invoice.Subtotal +
                invoice.TaxAmount -
                invoice.DiscountAmount;

            if (invoice.TotalAmount < 0)
            {
                return BadRequest(
                    "Total amount cannot be negative"
                );
            }

            invoice.IssueDate = DateTime.Now;

            ProjectContext.Invoices.Add(invoice);
            ProjectContext.SaveChanges();

            return Ok(new
            {
                Message = "Invoice added successfully",
                InvoiceId = invoice.InvoiceId,
                TotalAmount = invoice.TotalAmount
            });
        }
        // Get all invoices
        [HttpGet("GetAll")]
        public IActionResult GetAllInvoices()
        {
            var invoices = ProjectContext.Invoices
                .Select(i => new
                {
                    i.InvoiceId,
                    i.ServiceOrderId,
                    i.InvoiceNumber,
                    i.IssueDate,
                    i.DueDate,
                    i.Subtotal,
                    i.TaxAmount,
                    i.DiscountAmount,
                    i.TotalAmount,
                    i.Status,
                    i.Notes
                })
                .ToList();

            return Ok(invoices);
        }
        // Get invoice by ID
        [HttpGet("GetById/{id}")]
        public IActionResult GetInvoiceById(int id)
        {
            var invoice = ProjectContext.Invoices
                .Where(i => i.InvoiceId == id)
                .Select(i => new
                {
                    i.InvoiceId,
                    i.ServiceOrderId,
                    i.InvoiceNumber,
                    i.IssueDate,
                    i.DueDate,
                    i.Subtotal,
                    i.TaxAmount,
                    i.DiscountAmount,
                    i.TotalAmount,
                    i.Status,
                    i.Notes
                })
                .FirstOrDefault();

            if (invoice == null)
            {
                return NotFound("Invoice not found");
            }

            return Ok(invoice);
        }
