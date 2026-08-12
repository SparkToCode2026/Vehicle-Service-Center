import { Navigate, Outlet } from "react-router";
import { useAuth } from "../../context/AuthContext";
import { hasAllowedRole } from "../../utils/roleAccess";

function RoleRoute({ allowedRoles }) {
  const { user } = useAuth();

  const userHasPermission = hasAllowedRole(user, allowedRoles);

  if (!userHasPermission) {
    return <Navigate to="/unauthorized" replace />;
  }

  return <Outlet />;
}

export default RoleRoute;
