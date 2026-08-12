using VehicleServiceCenter.Models;

namespace VehicleServiceCenter.Services;

public interface IResourceAuthorizationService
{
    int? UserId { get; }
    bool IsAdmin { get; }
    bool IsCustomer { get; }
    bool IsMechanic { get; }

    bool CanAccessUser(int userId);
    int? GetCurrentMechanicProfileId();

    IQueryable<CustomerProfileModel> ScopeCustomerProfiles(
        IQueryable<CustomerProfileModel> query);
    IQueryable<MechanicProfileModel> ScopeMechanicProfiles(
        IQueryable<MechanicProfileModel> query);
    IQueryable<VehicleModel> ScopeVehicles(IQueryable<VehicleModel> query);
    IQueryable<AppointmentModel> ScopeAppointments(
        IQueryable<AppointmentModel> query);
    IQueryable<ServiceOrderModel> ScopeServiceOrders(
        IQueryable<ServiceOrderModel> query);
    IQueryable<ServiceOrderItemModel> ScopeServiceOrderItems(
        IQueryable<ServiceOrderItemModel> query);
    IQueryable<InvoiceModel> ScopeInvoices(IQueryable<InvoiceModel> query);
    IQueryable<PaymentModel> ScopePayments(IQueryable<PaymentModel> query);

    bool CanAccessCustomerProfile(int customerProfileId);
    bool CanAccessMechanicProfile(int mechanicProfileId);
    bool CanAccessVehicle(int vehicleId);
    bool CanAccessAppointment(int appointmentId);
    bool CanAccessServiceOrder(int serviceOrderId);
    bool CanAccessInvoice(int invoiceId);
    bool CanAccessPayment(int paymentId);
    bool CanManageServiceOrder(int serviceOrderId);
    bool CanManageServiceOrderItem(int serviceOrderItemId);
}
