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
