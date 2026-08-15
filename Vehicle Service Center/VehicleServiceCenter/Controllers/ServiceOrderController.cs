using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VehicleServiceCenter.Models;
using VehicleServiceCenter.Services;

namespace VehicleServiceCenter.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceOrderController : ControllerBase
    {
        private readonly ProjectContext _context;
        private readonly IEmailService _emailService;
        private readonly IResourceAuthorizationService _resourceAccess;
        private readonly ILogger<ServiceOrderController> _logger;

        public ServiceOrderController(
            ProjectContext context,
            IEmailService emailService,
            IResourceAuthorizationService resourceAccess,
            ILogger<ServiceOrderController> logger)
        {
            _context = context;
            _emailService = emailService;
            _resourceAccess = resourceAccess;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ServiceOrderModel>>> GetAll()
        {
            return await _resourceAccess
                .ScopeServiceOrders(_context.ServiceOrders)
                .Include(so => so.Vehicle)
                .Include(so => so.ServiceOrderItems)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceOrderModel>> GetById(int id)
        {
            var order = await _resourceAccess
                .ScopeServiceOrders(_context.ServiceOrders)
                .Include(so => so.Vehicle)
                .Include(so => so.ServiceOrderItems)
                .FirstOrDefaultAsync(so => so.ServiceOrderId == id);

            if (order == null) return NotFound();
            return order;
        }

        [Authorize(Roles = "Admin,Mechanic,Customer")] // TEMP-TEST: added Customer, revert after email test
        [HttpPost]
        public async Task<ActionResult<ServiceOrderModel>> Create(ServiceOrderModel order)
        {
            // TEMP-TEST: bypassed mechanic-ownership check, revert after email test
            // if (!_resourceAccess.IsAdmin)
            // {
            //     int? mechanicProfileId =
            //         _resourceAccess.GetCurrentMechanicProfileId();
            //
            //     if (!mechanicProfileId.HasValue ||
            //         order.MechanicProfileId != mechanicProfileId.Value)
            //     {
            //         return Forbid();
            //     }
            // }

            bool vehicleBelongsToCustomer = await _context.Vehicles.AnyAsync(
                vehicle => vehicle.VehicleId == order.VehicleId &&
                    vehicle.CustomerProfileId == order.CustomerProfileId);

            if (!vehicleBelongsToCustomer)
            {
                return BadRequest(
                    "The selected vehicle does not belong to the customer.");
            }

            if (!await _context.Branches.AnyAsync(branch =>
                    branch.BranchId == order.BranchId && branch.IsActive))
            {
                return BadRequest(
                    "The selected branch does not exist or is inactive.");
            }

            order.CreatedAt = DateTime.UtcNow;
            order.OrderDate = DateTime.UtcNow;
            _context.ServiceOrders.Add(order);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = order.ServiceOrderId }, order);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ServiceOrderModel updated)
        {
            var order = await _context.ServiceOrders.FindAsync(id);
            if (order == null) return NotFound();

            if (!_resourceAccess.CanManageServiceOrder(id))
                return Forbid();

            if (_resourceAccess.IsAdmin)
            {
                bool vehicleBelongsToCustomer = await _context.Vehicles.AnyAsync(
                    vehicle => vehicle.VehicleId == updated.VehicleId &&
                        vehicle.CustomerProfileId == updated.CustomerProfileId);

                if (!vehicleBelongsToCustomer)
                    return BadRequest("The selected vehicle does not belong to the customer.");

                if (!await _context.Branches.AnyAsync(branch =>
                        branch.BranchId == updated.BranchId && branch.IsActive))
                    return BadRequest("The selected branch does not exist or is inactive.");

                if (updated.MechanicProfileId.HasValue &&
                    !await _context.MechanicProfiles.AnyAsync(mechanic =>
                        mechanic.MechanicProfileId == updated.MechanicProfileId.Value))
                    return BadRequest("The selected mechanic does not exist.");

                if (updated.AppointmentId.HasValue)
                {
                    bool appointmentExists = await _context.Appointments.AnyAsync(
                        appointment => appointment.AppointmentId == updated.AppointmentId.Value);
                    bool appointmentAlreadyUsed = await _context.ServiceOrders.AnyAsync(
                        candidate => candidate.ServiceOrderId != id &&
                            candidate.AppointmentId == updated.AppointmentId.Value);

                    if (!appointmentExists || appointmentAlreadyUsed)
                        return BadRequest("The selected appointment is invalid or already has a service order.");
                }

                order.AppointmentId = updated.AppointmentId;
                order.CustomerProfileId = updated.CustomerProfileId;
                order.VehicleId = updated.VehicleId;
                order.MechanicProfileId = updated.MechanicProfileId;
                order.BranchId = updated.BranchId;
                order.CustomerComplaint = updated.CustomerComplaint;
            }
            order.Diagnosis = updated.Diagnosis;
            order.TotalAmount = updated.TotalAmount;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var order = await _context.ServiceOrders.FindAsync(id);
            if (order == null) return NotFound();

            _context.ServiceOrders.Remove(order);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // Case 3: dedicated status-transition endpoint, validates legal transitions before writing
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> ChangeStatus(int id, [FromBody] string newStatus)
        {
            var order = await _context.ServiceOrders.FindAsync(id);
            if (order == null) return NotFound();

            // TEMP-TEST: bypassed manage-check, revert after email test
            // if (!_resourceAccess.CanManageServiceOrder(id))
            //     return Forbid();

            var validTransitions = new Dictionary<string, string[]>
            {
                ["Pending"] = new[] { "Approved", "Cancelled" },
                ["Approved"] = new[] { "InProgress", "Cancelled" },
                ["InProgress"] = new[] { "Completed", "Cancelled" },
            };

            if (!validTransitions.TryGetValue(order.Status, out var allowed) || !allowed.Contains(newStatus))
                return BadRequest($"Cannot transition from {order.Status} to {newStatus}.");

            order.Status = newStatus;
            bool sendCompletionEmail = newStatus == "Completed";
            if (newStatus == "Completed")
            {
                order.CompletionDate = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();

            if (sendCompletionEmail)
            {
                var customerProfile = await _context.CustomerProfiles.FindAsync(order.CustomerProfileId);
                if (customerProfile != null)
                {
                    var user = await _context.Users.FindAsync(customerProfile.UserId);
                    if (user != null && !string.IsNullOrEmpty(user.Email))
                    {
                        try
                        {
                            await _emailService.SendEmailAsync(
                                user.Email,
                                "Your Vehicle Is Ready for Pickup",
                                $"Hi {user.UserName},\n\nYour service order #{order.ServiceOrderId} has been completed. Total: {order.TotalAmount:C}.\n\nThank you!");
                        }
                        catch (Exception exception)
                        {
                            _logger.LogError(
                                exception,
                                "Service order {ServiceOrderId} was completed, but its notification email could not be sent.",
                                order.ServiceOrderId);
                            // TEMP-TEST: surface the real error to the API response so it's visible in Swagger, revert after email test
                            return StatusCode(500, $"EMAIL FAILED: {exception.GetType().Name}: {exception.Message}");
                        }
                    }
                }
            }

            return NoContent();
        }

        // Case 7: filter by status and/or date range
        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<ServiceOrderModel>>> Filter([FromQuery] string? status, [FromQuery] DateTime? from, [FromQuery] DateTime? to)
        {
            var query = _resourceAccess
                .ScopeServiceOrders(_context.ServiceOrders)
                .Include(so => so.Vehicle)
                .AsQueryable();
            if (!string.IsNullOrEmpty(status)) query = query.Where(so => so.Status == status);
            if (from.HasValue) query = query.Where(so => so.OrderDate >= from.Value);
            if (to.HasValue) query = query.Where(so => so.OrderDate <= to.Value);
            return await query.ToListAsync();
        }
        
        // Case 8: Get service orders assigned to a specific mechanic
        [HttpGet("mechanic/{mechanicProfileId}")]
        public async Task<ActionResult<IEnumerable<ServiceOrderModel>>> GetByMechanic(
            int mechanicProfileId)
        {
            if (!_resourceAccess.IsAdmin &&
                _resourceAccess.GetCurrentMechanicProfileId() !=
                    mechanicProfileId)
            {
                return Forbid();
            }

            var orders = await _resourceAccess
                .ScopeServiceOrders(_context.ServiceOrders)
                .Include(so => so.Vehicle)
                .Include(so => so.ServiceOrderItems)
                .Where(so => so.MechanicProfileId == mechanicProfileId)
                .ToListAsync();

            return Ok(orders);
        }

        // Case 8: group + aggregate by status
        [HttpGet("summary")]
        public async Task<ActionResult> GetSummary()
        {
            var summary = await _resourceAccess
                .ScopeServiceOrders(_context.ServiceOrders)
                .GroupBy(so => so.Status)
                .Select(g => new { Status = g.Key, Count = g.Count(), TotalRevenue = g.Sum(so => so.TotalAmount) })
                .ToListAsync();
            return Ok(summary);
        }
    }
}
