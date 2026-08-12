export function hasAllowedRole(user, allowedRoles) {
  return Boolean(user?.role && allowedRoles.includes(user.role));
}

export function getHomePageForRole(role) {
  if (role === "Admin") return "/admin";
  if (role === "Customer") return "/customer";
  if (role === "Mechanic") return "/mechanic";
  return "/";
}
