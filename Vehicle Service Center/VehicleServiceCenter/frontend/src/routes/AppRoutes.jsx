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

function HomeRedirect() {
  const { user } = useAuth();

  if (user?.role === "Admin") {
    return <Navigate to="/admin" replace />;
  }

  return <Navigate to="/service-orders" replace />;
}

function AppRoutes() {
  return (
    <Routes>
      <Route path="/login" element={<Login />} />
      <Route path="/register" element={<Register />} />

      <Route element={<ProtectedRoute />}>
        <Route element={<AppLayout />}>
          <Route index element={<HomeRedirect />} />

          <Route element={<RoleRoute allowedRoles={["Admin"]} />}>
            <Route path="admin" element={<AdminDashboard />} />
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
