using Microsoft.AspNetCore.Mvc;
using VehicleServiceCenter.Models;
using Microsoft.EntityFrameworkCore;

namespace VehicleServiceCenter.Controllers
{
    [ApiController]
    [Route("Invoice")]
    public class InvoiceController : ControllerBase
    {
        private ProjectContext context;

        public InvoiceController(ProjectContext context)
        {
            this.context = context;
        }

        // Add invoice
        [HttpPost("AddInvoice")]
        public IActionResult AddInvoice(InvoiceModel invoice)
        {
            // Check whether the service order exists
            ServiceOrderModel serviceOrder =
                context.ServiceOrders.FirstOrDefault(s =>
                    s.ServiceOrderId == invoice.ServiceOrderId
                );

            if (serviceOrder == null)
            {
                return BadRequest(
                    "Service order does not exist"
                );
            }
            
            // Check whether this service order already has an invoice
            // (ServiceOrderId has a unique index, so this prevents a raw DB exception)
            InvoiceModel existingInvoiceForOrder =
                context.Invoices.FirstOrDefault(i =>
                    i.ServiceOrderId == invoice.ServiceOrderId
                );
 
            if (existingInvoiceForOrder != null)
            {
                return BadRequest(
                    "This service order already has an invoice"
                );
            }
            
            
            // Check whether invoice number already exists
            InvoiceModel existingInvoice =
                context.Invoices.FirstOrDefault(i =>
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

            context.Invoices.Add(invoice);
            context.SaveChanges();

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
            var invoices = context.Invoices
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
            var invoice = context.Invoices
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
            var invoice = context.Invoices
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
        
        // Filter invoices by status and/or issue date range
        [HttpGet("Filter")]
        public IActionResult FilterInvoices(
            string? status,
            DateTime? fromDate,
            DateTime? toDate
        )
        {
            if (string.IsNullOrWhiteSpace(status) &&
                !fromDate.HasValue &&
                !toDate.HasValue)
            {
                return BadRequest(
                    "Provide a status or a date range"
                );
            }
 
            IQueryable<InvoiceModel> query = context.Invoices;
 
            if (!string.IsNullOrWhiteSpace(status))
            {
                query = query.Where(i => i.Status == status);
            }
 
            if (fromDate.HasValue)
            {
                query = query.Where(i => i.IssueDate >= fromDate.Value);
            }
 
            if (toDate.HasValue)
            {
                query = query.Where(i => i.IssueDate <= toDate.Value);
            }
 
            var invoices = query
                .Select(i => new
                {
                    i.InvoiceId,
                    i.InvoiceNumber,
                    i.IssueDate,
                    i.TotalAmount,
                    i.Status
                })
                .ToList();
 
            return Ok(invoices);
        }
        
        // Sort invoices by total amount
        [HttpGet("SortByTotalAmount")]
        public IActionResult SortInvoicesByTotalAmount(
            bool descending = true
        )
        {
            IQueryable<InvoiceModel> query = context.Invoices;
 
            query = descending
                ? query.OrderByDescending(i => i.TotalAmount)
                : query.OrderBy(i => i.TotalAmount);
 
            var invoices = query
                .Select(i => new
                {
                    i.InvoiceId,
                    i.InvoiceNumber,
                    i.TotalAmount,
                    i.Status
                })
                .ToList();
 
            return Ok(invoices);
        }
        
        // Revenue summary grouped by status
        [HttpGet("RevenueSummary")]
        public IActionResult GetRevenueSummary()
        {
            var summary = context.Invoices
                .GroupBy(i => i.Status)
                .Select(g => new
                {
                    Status = g.Key,
                    Count = g.Count(),
                    TotalAmount = g.Sum(i => i.TotalAmount)
                })
                .ToList();
 
            return Ok(summary);
        }
        
        // Update invoice by ID
        [HttpPut("Update/{id}")]
        public IActionResult UpdateInvoice(
            int id,
            InvoiceModel updatedInvoice
        )
        {
            InvoiceModel invoice =
                context.Invoices.Find(id);

            if (invoice == null)
            {
                return NotFound("Invoice not found");
            }

            ServiceOrderModel serviceOrder =
                context.ServiceOrders.FirstOrDefault(s =>
                    s.ServiceOrderId ==
                    updatedInvoice.ServiceOrderId
                );

            if (serviceOrder == null)
            {
                return BadRequest(
                    "Service order does not exist"
                );
            }
            
            InvoiceModel existingInvoiceForOrder =
                context.Invoices.FirstOrDefault(i =>
                    i.ServiceOrderId ==
                    updatedInvoice.ServiceOrderId &&
                    i.InvoiceId != id
                );
 
            if (existingInvoiceForOrder != null)
            {
                return BadRequest(
                    "This service order already has an invoice"
                );
            }

            InvoiceModel existingInvoice =
                context.Invoices.FirstOrDefault(i =>
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
            context.SaveChanges();

            return Ok(new
            {
                Message = "Invoice updated successfully",
                InvoiceId = invoice.InvoiceId,
                TotalAmount = invoice.TotalAmount
            });
        }
        // Change invoice status
        [HttpPatch("ChangeStatus/{id}")]
        public IActionResult ChangeInvoiceStatus(
            int id,
            string status
        )
        {
            InvoiceModel invoice =
                context.Invoices.Find(id);

            if (invoice == null)
            {
                return NotFound("Invoice not found");
            }

            invoice.Status = status;
            context.SaveChanges();

            return Ok(new
            {
                Message = "Invoice status changed successfully",
                InvoiceId = invoice.InvoiceId,
                Status = invoice.Status
            });
        }
        // Delete invoice by ID
        [HttpDelete("Delete/{id}")]
        public IActionResult DeleteInvoice(int id)
        {
            InvoiceModel invoice =
                context.Invoices.Find(id);

            if (invoice == null)
            {
                return NotFound("Invoice not found");
            }

            bool hasPayments = context.Payments
                .Any(p => p.InvoiceId == id);

            if (hasPayments)
            {
                return BadRequest(
                    "Invoice cannot be deleted because it has payments"
                );
            }

            context.Invoices.Remove(invoice);
            context.SaveChanges();

            return Ok(new
            {
                Message = "Invoice deleted successfully",
                InvoiceId = invoice.InvoiceId
            });
        }
    }
}


