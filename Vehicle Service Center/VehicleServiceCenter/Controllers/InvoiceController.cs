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
        // Get invoice by service order ID
        [HttpGet("GetByServiceOrderId/{serviceOrderId}")]
        public IActionResult GetInvoiceByServiceOrderId(
            int serviceOrderId
        )
        {
            var invoice = ProjectContext.Invoices
                .Where(i =>
                    i.ServiceOrderId == serviceOrderId
                )
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
                return NotFound(
                    "Invoice not found for this service order"
                );
            }

            return Ok(invoice);
        }
        // Update invoice by ID
        [HttpPut("Update/{id}")]
        public IActionResult UpdateInvoice(
            int id,
            InvoiceModel updatedInvoice
        )
        {
            InvoiceModel invoice =
                ProjectContext.Invoices.Find(id);

            if (invoice == null)
            {
                return NotFound("Invoice not found");
            }

            ServiceOrderModel serviceOrder =
                ProjectContext.ServiceOrders.FirstOrDefault(s =>
                    s.ServiceOrderId ==
                    updatedInvoice.ServiceOrderId
                );

            if (serviceOrder == null)
            {
                return BadRequest(
                    "Service order does not exist"
                );
            }

            InvoiceModel existingInvoice =
                ProjectContext.Invoices.FirstOrDefault(i =>
                    i.InvoiceNumber ==
                        updatedInvoice.InvoiceNumber &&
                    i.InvoiceId != id
                );

            if (existingInvoice != null)
            {
                return BadRequest(
                    "Invoice number already exists"
                );
            }

            if (updatedInvoice.Subtotal < 0 ||
                updatedInvoice.TaxAmount < 0 ||
                updatedInvoice.DiscountAmount < 0)
            {
                return BadRequest(
                    "Invoice amounts cannot be negative"
                );
            }

            decimal totalAmount =
                updatedInvoice.Subtotal +
                updatedInvoice.TaxAmount -
                updatedInvoice.DiscountAmount;

            if (totalAmount < 0)
            {
                return BadRequest(
                    "Total amount cannot be negative"
                );
            }

            invoice.ServiceOrderId =
                updatedInvoice.ServiceOrderId;
            invoice.InvoiceNumber =
                updatedInvoice.InvoiceNumber;
            invoice.IssueDate =
                updatedInvoice.IssueDate;
            invoice.DueDate =
                updatedInvoice.DueDate;
            invoice.Subtotal =
                updatedInvoice.Subtotal;
            invoice.TaxAmount =
                updatedInvoice.TaxAmount;
            invoice.DiscountAmount =
                updatedInvoice.DiscountAmount;
            invoice.TotalAmount = totalAmount;
            invoice.Status = updatedInvoice.Status;
            invoice.Notes = updatedInvoice.Notes;


