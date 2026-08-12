using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VehicleServiceCenter.Models;
using VehicleServiceCenter.Services;

namespace VehicleServiceCenter.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class AppointmentController : ControllerBase
{
    private static readonly HashSet<string> AllowedStatuses = new(
        StringComparer.OrdinalIgnoreCase)
    {
        "Pending",
        "Confirmed",
        "In Progress",
        "Completed",
        "Cancelled"
    };

    private readonly ProjectContext context;
    private readonly IResourceAuthorizationService resourceAccess;
    private readonly IEmailService emailService;
    private readonly ILogger<AppointmentController> logger;

    public AppointmentController(
        ProjectContext context,
        IResourceAuthorizationService resourceAccess,
        IEmailService emailService,
        ILogger<AppointmentController> logger)
    {
        this.context = context;
        this.resourceAccess = resourceAccess;
        this.emailService = emailService;
        this.logger = logger;
    }

    // 1. POST - Create a new appointment
    [Authorize(Roles = "Admin,Customer")]
    [HttpPost]
    public async Task<IActionResult> CreateAppointment(
        AppointmentModel appointment)
    {
        if (!context.CustomerProfiles.Any(profile =>
                profile.CustomerProfileId == appointment.CustomerProfileId))
        {
            return BadRequest("Customer profile does not exist.");
        }

        if (!resourceAccess.IsAdmin &&
            !resourceAccess.CanAccessCustomerProfile(
                appointment.CustomerProfileId))
        {
            return Forbid();
        }

        string? validationError = ValidateReferences(appointment);
        if (validationError != null)
        {
            return BadRequest(validationError);
        }

        appointment.AppointmentId = 0;
        appointment.CreatedAt = DateTime.UtcNow;
        appointment.Status = resourceAccess.IsAdmin &&
            AllowedStatuses.Contains(appointment.Status)
                ? appointment.Status
                : "Pending";

        context.Appointments.Add(appointment);
        await context.SaveChangesAsync();

        await SendConfirmationEmailAsync(appointment);

        return CreatedAtAction(
            nameof(GetAppointment),
            new { id = appointment.AppointmentId },
            appointment);
    }

    // 2. PUT - Update appointment details
    [Authorize(Roles = "Admin,Customer")]
    [HttpPut("{id}")]
    public IActionResult UpdateAppointment(
        int id,
        AppointmentModel appointment)
    {
        AppointmentModel? existingAppointment =
            context.Appointments.Find(id);

        if (existingAppointment == null)
        {
            return NotFound();
        }

        if (!resourceAccess.IsAdmin &&
            !resourceAccess.CanAccessAppointment(id))
        {
            return Forbid();
        }

        int customerProfileId = resourceAccess.IsAdmin
            ? appointment.CustomerProfileId
            : existingAppointment.CustomerProfileId;

        appointment.CustomerProfileId = customerProfileId;
        if (!resourceAccess.IsAdmin)
        {
            appointment.MechanicProfileId =
                existingAppointment.MechanicProfileId;
            appointment.Status = existingAppointment.Status;
        }

        string? validationError = ValidateReferences(appointment);
        if (validationError != null)
        {
            return BadRequest(validationError);
        }

        existingAppointment.CustomerProfileId = customerProfileId;
        existingAppointment.VehicleId = appointment.VehicleId;
        existingAppointment.ServiceTypeId = appointment.ServiceTypeId;
        existingAppointment.BranchId = appointment.BranchId;
        existingAppointment.AppointmentDate = appointment.AppointmentDate;
        existingAppointment.Notes = appointment.Notes;

        if (resourceAccess.IsAdmin)
        {
            existingAppointment.MechanicProfileId =
                appointment.MechanicProfileId;

            if (!AllowedStatuses.Contains(appointment.Status))
            {
                return BadRequest("Invalid appointment status.");
            }

            existingAppointment.Status = appointment.Status;
        }

        context.SaveChanges();
        return NoContent();
    }

    // 3. PATCH - Change appointment status
    [HttpPatch("{id}/status")]
    public IActionResult ChangeAppointmentStatus(int id, string status)
    {
        AppointmentModel? appointment = context.Appointments.Find(id);

        if (appointment == null)
        {
            return NotFound();
        }

        bool canChangeStatus = resourceAccess.IsAdmin ||
            (resourceAccess.IsMechanic &&
             resourceAccess.CanAccessAppointment(id)) ||
            (resourceAccess.IsCustomer &&
             resourceAccess.CanAccessAppointment(id) &&
             status.Equals("Cancelled", StringComparison.OrdinalIgnoreCase));

        if (!canChangeStatus)
        {
            return Forbid();
        }

        if (!AllowedStatuses.Contains(status))
        {
            return BadRequest("Invalid appointment status.");
        }

        appointment.Status = status;
        context.SaveChanges();
        return Ok(appointment);
    }

    // 4. DELETE - Delete an appointment
    [Authorize(Roles = "Admin,Customer")]
    [HttpDelete("{id}")]
    public IActionResult DeleteAppointment(int id)
    {
        AppointmentModel? appointment = context.Appointments.Find(id);

        if (appointment == null)
        {
            return NotFound();
        }

        if (!resourceAccess.IsAdmin &&
            !resourceAccess.CanAccessAppointment(id))
        {
            return Forbid();
        }

        List<ServiceOrderModel> serviceOrders = context.ServiceOrders
            .Where(order => order.AppointmentId == id)
            .ToList();

        foreach (ServiceOrderModel serviceOrder in serviceOrders)
        {
            serviceOrder.AppointmentId = null;
        }

        context.Appointments.Remove(appointment);
        context.SaveChanges();
        return NoContent();
    }

    // 5. GET - Get all appointments with related entities
    [HttpGet]
    public IActionResult GetAppointments()
    {
        List<AppointmentModel> appointments = resourceAccess
            .ScopeAppointments(context.Appointments)
            .Include(appointment => appointment.CustomerProfile)
            .Include(appointment => appointment.Vehicle)
            .Include(appointment => appointment.ServiceType)
            .Include(appointment => appointment.MechanicProfile)
            .Include(appointment => appointment.Branch)
            .ToList();

        return Ok(appointments);
    }

    // 6. GET - Get appointment by ID
    [HttpGet("{id}")]
    public IActionResult GetAppointment(int id)
    {
        AppointmentModel? appointment = resourceAccess
            .ScopeAppointments(context.Appointments)
            .Include(item => item.CustomerProfile)
            .Include(item => item.Vehicle)
            .Include(item => item.ServiceType)
            .Include(item => item.MechanicProfile)
            .Include(item => item.Branch)
            .FirstOrDefault(item => item.AppointmentId == id);

        return appointment == null ? NotFound() : Ok(appointment);
    }

    // 7. GET - Filter appointments by status
    [HttpGet("filter")]
    public IActionResult GetAppointmentsByStatus(string status)
    {
        List<AppointmentModel> appointments = resourceAccess
            .ScopeAppointments(context.Appointments)
            .Where(appointment => appointment.Status == status)
            .Include(appointment => appointment.CustomerProfile)
            .Include(appointment => appointment.Vehicle)
            .Include(appointment => appointment.ServiceType)
            .ToList();

        return Ok(appointments);
    }

    // 8. GET - Sort appointments by date
    [HttpGet("sort")]
    public IActionResult GetAppointmentsSorted()
    {
        List<AppointmentModel> appointments = resourceAccess
            .ScopeAppointments(context.Appointments)
            .OrderBy(appointment => appointment.AppointmentDate)
            .Include(appointment => appointment.CustomerProfile)
            .Include(appointment => appointment.Vehicle)
            .Include(appointment => appointment.ServiceType)
            .ToList();

        return Ok(appointments);
    }

    private string? ValidateReferences(AppointmentModel appointment)
    {
        bool vehicleBelongsToCustomer = context.Vehicles.Any(vehicle =>
            vehicle.VehicleId == appointment.VehicleId &&
            vehicle.CustomerProfileId == appointment.CustomerProfileId);

        if (!vehicleBelongsToCustomer)
        {
            return "The selected vehicle does not belong to the customer.";
        }

        if (!context.ServiceTypes.Any(service =>
                service.ServiceTypeId == appointment.ServiceTypeId &&
                service.IsActive))
        {
            return "The selected service type does not exist or is inactive.";
        }

        if (!context.Branches.Any(branch =>
                branch.BranchId == appointment.BranchId &&
                branch.IsActive))
        {
            return "The selected branch does not exist or is inactive.";
        }

        if (appointment.MechanicProfileId.HasValue &&
            !context.MechanicProfiles.Any(mechanic =>
                mechanic.MechanicProfileId ==
                    appointment.MechanicProfileId.Value &&
                mechanic.BranchId == appointment.BranchId))
        {
            return "The selected mechanic does not belong to this branch.";
        }

        return null;
    }

    private async Task SendConfirmationEmailAsync(
        AppointmentModel appointment)
    {
        var details = await context.CustomerProfiles
            .Where(profile => profile.CustomerProfileId ==
                appointment.CustomerProfileId)
            .Select(profile => new
            {
                UserName = profile.User!.UserName,
                Email = profile.User!.Email,
                Vehicle = context.Vehicles
                    .Where(vehicle => vehicle.VehicleId ==
                        appointment.VehicleId)
                    .Select(vehicle => vehicle.Make + " " + vehicle.Model)
                    .First(),
                Service = context.ServiceTypes
                    .Where(service => service.ServiceTypeId ==
                        appointment.ServiceTypeId)
                    .Select(service => service.Name)
                    .First(),
                Branch = context.Branches
                    .Where(branch => branch.BranchId ==
                        appointment.BranchId)
                    .Select(branch => branch.BranchName)
                    .First()
            })
            .FirstOrDefaultAsync();

        if (details == null || string.IsNullOrWhiteSpace(details.Email))
        {
            logger.LogWarning(
                "Appointment {AppointmentId} was created without a confirmation email because no recipient was available.",
                appointment.AppointmentId);
            return;
        }

        string body =
            $"Hi {details.UserName},\n\n" +
            $"Your appointment #{appointment.AppointmentId} is confirmed.\n" +
            $"Date: {appointment.AppointmentDate:f}\n" +
            $"Vehicle: {details.Vehicle}\n" +
            $"Service: {details.Service}\n" +
            $"Branch: {details.Branch}\n" +
            $"Status: {appointment.Status}\n\n" +
            "Thank you.";

        try
        {
            await emailService.SendEmailAsync(
                details.Email,
                $"Appointment #{appointment.AppointmentId} Confirmation",
                body);
        }
        catch (Exception exception)
        {
            logger.LogError(
                exception,
                "Appointment {AppointmentId} was saved, but its confirmation email could not be sent.",
                appointment.AppointmentId);
        }
    }
}
