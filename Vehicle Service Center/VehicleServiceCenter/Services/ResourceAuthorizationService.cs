using System.Security.Claims;
using VehicleServiceCenter.Models;

namespace VehicleServiceCenter.Services;

public sealed class ResourceAuthorizationService : IResourceAuthorizationService
{
    private readonly ProjectContext _context;
    private readonly ClaimsPrincipal _principal;

    public ResourceAuthorizationService(
        ProjectContext context,
        IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _principal = httpContextAccessor.HttpContext?.User
            ?? new ClaimsPrincipal();
    }

    public int? UserId
    {
        get
        {
            string? value = _principal.FindFirstValue(
                ClaimTypes.NameIdentifier);
            return int.TryParse(value, out int userId) ? userId : null;
        }
    }

    public bool IsAdmin => _principal.IsInRole("Admin");
    public bool IsCustomer => _principal.IsInRole("Customer");
    public bool IsMechanic => _principal.IsInRole("Mechanic");

    public bool CanAccessUser(int userId) =>
        IsAdmin || UserId == userId;

    public int? GetCurrentMechanicProfileId()
    {
        if (!IsMechanic || !UserId.HasValue)
        {
            return null;
        }

        return _context.MechanicProfiles
            .Where(profile => profile.UserId == UserId.Value)
            .Select(profile => (int?)profile.MechanicProfileId)
            .FirstOrDefault();
    }

    public IQueryable<CustomerProfileModel> ScopeCustomerProfiles(
        IQueryable<CustomerProfileModel> query)
    {
        if (IsAdmin)
        {
            return query;
        }

        return IsCustomer && UserId.HasValue
            ? query.Where(profile => profile.UserId == UserId.Value)
            : query.Where(_ => false);
    }

    public IQueryable<MechanicProfileModel> ScopeMechanicProfiles(
        IQueryable<MechanicProfileModel> query)
    {
        if (IsAdmin)
        {
            return query;
        }

        return IsMechanic && UserId.HasValue
            ? query.Where(profile => profile.UserId == UserId.Value)
            : query.Where(_ => false);
    }

    public IQueryable<VehicleModel> ScopeVehicles(
        IQueryable<VehicleModel> query)
    {
        if (IsAdmin)
        {
            return query;
        }

        if (IsCustomer && UserId.HasValue)
        {
            return query.Where(vehicle =>
                vehicle.CustomerProfile.UserId == UserId.Value);
        }

        if (IsMechanic && UserId.HasValue)
        {
            return query.Where(vehicle =>
                vehicle.ServiceOrders.Any(order =>
                    order.MechanicProfile != null &&
                    order.MechanicProfile.UserId == UserId.Value) ||
                vehicle.Appointments.Any(appointment =>
                    appointment.MechanicProfile != null &&
                    appointment.MechanicProfile.UserId == UserId.Value));
        }

        return query.Where(_ => false);
    }

    public IQueryable<AppointmentModel> ScopeAppointments(
        IQueryable<AppointmentModel> query)
    {
        if (IsAdmin)
        {
            return query;
        }

        if (IsCustomer && UserId.HasValue)
        {
            return query.Where(appointment =>
                appointment.CustomerProfile.UserId == UserId.Value);
        }

        if (IsMechanic && UserId.HasValue)
        {
            return query.Where(appointment =>
                appointment.MechanicProfile != null &&
                appointment.MechanicProfile.UserId == UserId.Value);
        }

        return query.Where(_ => false);
    }

    public IQueryable<ServiceOrderModel> ScopeServiceOrders(
        IQueryable<ServiceOrderModel> query)
    {
        if (IsAdmin)
        {
            return query;
        }

        if (IsCustomer && UserId.HasValue)
        {
            return query.Where(order =>
                order.CustomerProfile.UserId == UserId.Value);
        }

        if (IsMechanic && UserId.HasValue)
        {
            return query.Where(order =>
                order.MechanicProfile != null &&
                order.MechanicProfile.UserId == UserId.Value);
        }

        return query.Where(_ => false);
    }

    public IQueryable<ServiceOrderItemModel> ScopeServiceOrderItems(
        IQueryable<ServiceOrderItemModel> query)
    {
        IQueryable<int> serviceOrderIds = ScopeServiceOrders(
                _context.ServiceOrders)
            .Select(order => order.ServiceOrderId);

        return query.Where(item => serviceOrderIds.Contains(
            item.ServiceOrderId));
    }

    public IQueryable<InvoiceModel> ScopeInvoices(
        IQueryable<InvoiceModel> query)
    {
        IQueryable<int> serviceOrderIds = ScopeServiceOrders(
                _context.ServiceOrders)
            .Select(order => order.ServiceOrderId);

        return query.Where(invoice => serviceOrderIds.Contains(
            invoice.ServiceOrderId));
    }

    public IQueryable<PaymentModel> ScopePayments(
        IQueryable<PaymentModel> query)
    {
        IQueryable<int> invoiceIds = ScopeInvoices(_context.Invoices)
            .Select(invoice => invoice.InvoiceId);

        return query.Where(payment => invoiceIds.Contains(
            payment.InvoiceId));
    }

    public bool CanAccessCustomerProfile(int customerProfileId) =>
        ScopeCustomerProfiles(_context.CustomerProfiles)
            .Any(profile => profile.CustomerProfileId == customerProfileId);

    public bool CanAccessMechanicProfile(int mechanicProfileId) =>
        ScopeMechanicProfiles(_context.MechanicProfiles)
            .Any(profile => profile.MechanicProfileId == mechanicProfileId);

    public bool CanAccessVehicle(int vehicleId) =>
        ScopeVehicles(_context.Vehicles)
            .Any(vehicle => vehicle.VehicleId == vehicleId);

    public bool CanAccessAppointment(int appointmentId) =>
        ScopeAppointments(_context.Appointments)
            .Any(appointment => appointment.AppointmentId == appointmentId);

    public bool CanAccessServiceOrder(int serviceOrderId) =>
        ScopeServiceOrders(_context.ServiceOrders)
            .Any(order => order.ServiceOrderId == serviceOrderId);

    public bool CanAccessInvoice(int invoiceId) =>
        ScopeInvoices(_context.Invoices)
            .Any(invoice => invoice.InvoiceId == invoiceId);

    public bool CanAccessPayment(int paymentId) =>
        ScopePayments(_context.Payments)
            .Any(payment => payment.PaymentId == paymentId);

    public bool CanManageServiceOrder(int serviceOrderId)
    {
        if (IsAdmin)
        {
            return true;
        }

        return IsMechanic && CanAccessServiceOrder(serviceOrderId);
    }

    public bool CanManageServiceOrderItem(int serviceOrderItemId)
    {
        if (IsAdmin)
        {
            return true;
        }

        return IsMechanic && ScopeServiceOrderItems(
                _context.ServiceOrderItems)
            .Any(item => item.ServiceOrderItemId == serviceOrderItemId);
    }
}
