import { Navigate, Outlet } from "react-router";
import { useAuth } from "../../context/AuthContext";

function RoleRoute({ allowedRoles }) {
  const { user } = useAuth();

  const userHasPermission = allowedRoles.includes(user?.role);

  if (!userHasPermission) {
    return <Navigate to="/" replace />;
  }

  return <Outlet />;
}

export default RoleRoute;
