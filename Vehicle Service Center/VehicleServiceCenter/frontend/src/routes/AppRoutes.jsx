import { Navigate, Route, Routes } from "react-router";
import AppLayout from "../components/layout/AppLayout";
import ProtectedRoute from "../components/routing/ProtectedRoute";
import RoleRoute from "../components/routing/RoleRoute";
import { useAuth } from "../context/AuthContext";
import AccountSettings from "../pages/account/AccountSettings";
import AdminDashboard from "../pages/admin/AdminDashboard";
import BranchManagement from "../pages/admin/BranchManagement";
import CustomerProfileManagement from "../pages/admin/CustomerProfileManagement";
import InvoiceManagement from "../pages/admin/InvoiceManagement";
import MechanicManagement from "../pages/admin/MechanicManagement";
import PaymentManagement from "../pages/admin/PaymentManagement";
import ServiceTypeManagement from "../pages/admin/ServiceTypeManagement";
import SparePartManagement from "../pages/admin/SparePartManagement";
import UserManagement from "../pages/admin/UserManagement";
import VehicleManagement from "../pages/admin/VehicleManagement";
import AppointmentBooking from "../pages/appointments/AppointmentBooking";
import AppointmentDetails from "../pages/appointments/AppointmentDetails";
import AppointmentList from "../pages/appointments/AppointmentList";
import AppointmentManagement from "../pages/appointments/AppointmentManagement";
import Login from "../pages/auth/Login";
import Register from "../pages/auth/Register";
import Unauthorized from "../pages/auth/Unauthorized";
import InvoiceDetails from "../pages/billing/InvoiceDetails";
import PaymentDetails from "../pages/billing/PaymentDetails";
import CustomerDashboard from "../pages/customer/CustomerDashboard";
import CustomerBilling from "../pages/customer/CustomerBilling";
import CustomerProfile from "../pages/customer/CustomerProfile";
import VehicleDetails from "../pages/customer/VehicleDetails";
import VehicleFormPage from "../pages/customer/VehicleFormPage";
import VehicleList from "../pages/customer/VehicleList";
import SparePartsList from "../pages/inventory/SparePartsList";
import MechanicDashboard from "../pages/mechanic/MechanicDashboard";
import MechanicAvailability from "../pages/mechanic/MechanicAvailability";
import MechanicBilling from "../pages/mechanic/MechanicBilling";
import About from "../pages/public/About";
import Branches from "../pages/public/Branches";
import Home from "../pages/public/Home";
import Services from "../pages/public/Services";
import ServiceOrderDetails from "../pages/serviceOrders/ServiceOrderDetails";
import ServiceOrderFormPage from "../pages/serviceOrders/ServiceOrderFormPage";
import ServiceOrderList from "../pages/serviceOrders/ServiceOrderList";

function RootRoute() {
  const { user } = useAuth();

  if (!user) {
    return <Home />;
  }

  if (user?.role === "Admin") {
    return <Navigate to="/admin" replace />;
  }

  if (user?.role === "Customer") {
    return <Navigate to="/customer" replace />;
  }

  if (user?.role === "Mechanic") {
    return <Navigate to="/mechanic" replace />;
  }

  return <Navigate to="/service-orders" replace />;
  }

function AppRoutes() {
  return (
    <Routes>
      <Route path="/login" element={<Login />} />
      <Route path="/register" element={<Register />} />
      
      <Route element={<AppLayout />}>
        <Route path="/" element={<RootRoute />} />
        <Route path="/about" element={<About />} />
        <Route path="/services" element={<Services />} />
        <Route path="/branches" element={<Branches />} />
      </Route>

      <Route element={<ProtectedRoute />}>
        <Route element={<AppLayout />}>
          <Route path="unauthorized" element={<Unauthorized />} />
          <Route path="account" element={<AccountSettings />} />

          <Route element={<RoleRoute allowedRoles={["Admin"]} />}>
            <Route path="admin" element={<AdminDashboard />} />
            <Route path="admin/users" element={<UserManagement />} />
            <Route path="admin/customers" element={<CustomerProfileManagement />} />
            <Route path="admin/vehicles" element={<VehicleManagement />} />
            <Route path="admin/service-types" element={<ServiceTypeManagement />} />
            <Route path="admin/mechanics" element={<MechanicManagement />} />
            <Route path="admin/branches" element={<BranchManagement />} />
            <Route path="admin/invoices" element={<InvoiceManagement />} />
            <Route path="admin/payments" element={<PaymentManagement />} />
            <Route path="admin/spare-parts" element={<SparePartManagement />} />
          </Route>

          <Route
              element={<RoleRoute allowedRoles={["Mechanic"]} />}
          >
            <Route path="mechanic" element={<MechanicDashboard />} />
          </Route>

          <Route
              element={
                <RoleRoute allowedRoles={["Admin", "Mechanic"]} />
              }
          >
            <Route
                path="appointments/management"
                element={<AppointmentManagement />}
            />
            <Route path="service-orders/new" element={<ServiceOrderFormPage />} />
            <Route path="service-orders/:id/edit" element={<ServiceOrderFormPage />} />
          </Route>

          <Route
              element={
                <RoleRoute allowedRoles={["Mechanic"]} />
              }
          >
            <Route
                path="mechanic/availability"
                element={<MechanicAvailability />}
            />
            <Route path="mechanic/billing" element={<MechanicBilling />} />
          </Route>
          
          

          <Route element={<RoleRoute allowedRoles={["Customer"]} />}>
            <Route path="customer" element={<CustomerDashboard />} />
            <Route
              path="customer/profile"
              element={<CustomerProfile />}
            />
            <Route
              path="customer/vehicles"
              element={<VehicleList />}
            />
            <Route
              path="customer/vehicles/new"
              element={<VehicleFormPage />}
            />
            <Route
              path="customer/vehicles/:id"
              element={<VehicleDetails />}
            />
            <Route
              path="customer/vehicles/:id/edit"
              element={<VehicleFormPage />}
            />

            <Route
                path="customer/appointments/new"
                element={<AppointmentBooking />}
            />
            <Route
                path="customer/appointments"
                element={<AppointmentList />}
            />
            <Route path="customer/billing" element={<CustomerBilling />} />
            
          </Route>

          <Route
            element={
              <RoleRoute
                allowedRoles={["Admin", "Customer", "Mechanic"]}
              />
            }
          >
            <Route path="service-orders" element={<ServiceOrderList />} />
            <Route
              path="service-orders/:id"
              element={<ServiceOrderDetails />}
            />
            <Route
              path="appointments/:id"
              element={<AppointmentDetails />}
            />
          </Route>

          <Route
            element={<RoleRoute allowedRoles={["Admin", "Mechanic"]} />}
          >
            <Route path="spare-parts" element={<SparePartsList />} />
          </Route>

          <Route
            element={<RoleRoute allowedRoles={["Admin", "Customer", "Mechanic"]} />}
          >
            <Route path="invoices/:id" element={<InvoiceDetails />} />
            <Route path="payments/:id" element={<PaymentDetails />} />
          </Route>
        </Route>
      </Route>

      <Route path="*" element={<Navigate to="/" replace />} />
    </Routes>
  );
}

export default AppRoutes;
