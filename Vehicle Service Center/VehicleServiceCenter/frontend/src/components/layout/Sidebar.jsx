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
    <aside className="col-auto col-md-3 col-xl-2 bg-dark text-white p-3">
      <h1 className="fs-5 mb-4">Vehicle Service Center</h1>

      <nav aria-label="Main navigation">
        <ul className="nav nav-pills flex-column gap-2">
          {navigationLinks.map((link) => (
            <li className="nav-item" key={link.path}>
              <NavLink
                to={link.path}
                className={({ isActive }) =>
                  `nav-link ${isActive ? "active" : "text-white"}`
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
