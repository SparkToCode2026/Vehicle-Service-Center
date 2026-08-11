import { NavLink } from "react-router";
import { useAuth } from "../../context/AuthContext";

const navigationByRole = {
  Admin: [
    { label: "Dashboard", path: "/admin" },
    { label: "Service Orders", path: "/service-orders" },
    { label: "Spare Parts", path: "/spare-parts" },
    { label: "Invoices", path: "/invoices" },
    { label: "Payments", path: "/payments" },
  ],
  Customer: [
    { label: "Service Orders", path: "/service-orders" },
    { label: "Invoices", path: "/invoices" },
    { label: "Payments", path: "/payments" },
  ],
  Mechanic: [
    { label: "Service Orders", path: "/service-orders" },
    { label: "Spare Parts", path: "/spare-parts" },
  ],
};

function Sidebar() {
  const { user } = useAuth();
  const navigationLinks = navigationByRole[user?.role] || [];

  return (
    <aside className="sidebar">
      <h1>Vehicle Service Center</h1>

      <nav aria-label="Main navigation">
        <ul>
          {navigationLinks.map((link) => (
            <li key={link.path}>
              <NavLink
                to={link.path}
                className={({ isActive }) =>
                  isActive ? "active" : undefined
                }
              >
                {link.label}
              </NavLink>
            </li>
          ))}
        </ul>
      </nav>
    </aside>
  );
}

export default Sidebar;
