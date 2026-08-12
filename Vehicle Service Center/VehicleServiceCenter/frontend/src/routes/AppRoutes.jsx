import { Navigate, Route, Routes } from "react-router";
import AppLayout from "../components/layout/AppLayout";
import ProtectedRoute from "../components/routing/ProtectedRoute";
import RoleRoute from "../components/routing/RoleRoute";
import { useAuth } from "../context/AuthContext";
import AdminDashboard from "../pages/AdminDashboard";
import InvoiceDetails from "../pages/InvoiceDetails";
import Login from "../pages/Login";
import PaymentDetails from "../pages/PaymentDetails";
import Register from "../pages/Register";
import ServiceOrderDetails from "../pages/ServiceOrderDetails";
import ServiceOrderList from "../pages/ServiceOrderList";
import SparePartsList from "../pages/SparePartsList";
import Home from "../pages/Home";
import About from "../pages/About";
import Services from "../pages/Services";
import Branches from "../pages/Branches";
import CustomerDashboard from "../pages/customer/CustomerDashboard";
import CustomerProfile from "../pages/customer/CustomerProfile";
import VehicleDetails from "../pages/customer/VehicleDetails";
import VehicleFormPage from "../pages/customer/VehicleFormPage";
import VehicleList from "../pages/customer/VehicleList";

import AppointmentBooking from "../pages/appointments/AppointmentBooking";
import AppointmentList from "../pages/appointments/AppointmentList";
import AppointmentManagement from "../pages/appointments/AppointmentManagement";
import MechanicAvailability from "../pages/mechanic/MechanicAvailability";

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
          <Route element={<RoleRoute allowedRoles={["Admin"]} />}>
            <Route path="admin" element={<AdminDashboard />} />
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
          </Route>

          <Route
            element={<RoleRoute allowedRoles={["Admin", "Mechanic"]} />}
          >
            <Route path="spare-parts" element={<SparePartsList />} />
          </Route>

          <Route
            element={<RoleRoute allowedRoles={["Admin", "Customer"]} />}
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
