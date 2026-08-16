import { NavLink } from "react-router";
import { useAuth } from "../../context/AuthContext";
import BrandLogo from "./BrandLogo";

const navigationByRole = {
  Admin: [
    { label: "Dashboard", path: "/admin", icon: "speedometer2", end: true },
    { label: "Users", path: "/admin/users", icon: "people" },
    { label: "Customers", path: "/admin/customers", icon: "person-vcard" },
    { label: "Vehicles", path: "/admin/vehicles", icon: "car-front" },
    { label: "Service Types", path: "/admin/service-types", icon: "tools" },
    { label: "Mechanics", path: "/admin/mechanics", icon: "person-gear" },
    { label: "Branches", path: "/admin/branches", icon: "geo-alt" },
    { label: "Appointments", path: "/appointments/management", icon: "calendar-check" },
    { label: "Service Orders", path: "/service-orders", icon: "clipboard-check" },
    { label: "Inventory", path: "/admin/spare-parts", icon: "boxes" },
    { label: "Invoices", path: "/admin/invoices", icon: "receipt" },
    { label: "Payments", path: "/admin/payments", icon: "credit-card" },
  ],
  Customer: [
    { label: "Dashboard", path: "/customer", end: true, icon: "speedometer2" },
    { label: "Profile", path: "/customer/profile", icon: "person" },
    { label: "My Vehicles", path: "/customer/vehicles", icon: "car-front" },
    { label: "Service Orders", path: "/service-orders", icon: "clipboard-check" },
    { label: "My Appointments", path: "/customer/appointments", icon: "calendar3" },
    { label: "Book Appointment", path: "/customer/appointments/new", icon: "calendar-plus" },
    { label: "Billing", path: "/customer/billing", icon: "receipt" },
  ],
  Mechanic: [
    { label: "Dashboard", path: "/mechanic", end: true, icon: "speedometer2" },
    { label: "Availability", path: "/mechanic/availability", icon: "toggle-on" },
    { label: "Service Orders", path: "/service-orders", icon: "clipboard-check" },
    { label: "Appointments", path: "/appointments/management", icon: "calendar-check" },
    { label: "Spare Parts", path: "/spare-parts", icon: "boxes" },
    { label: "Billing Records", path: "/mechanic/billing", icon: "receipt" },
  ],
};

function Sidebar() {
  const { user } = useAuth();
  const navigationLinks = navigationByRole[user?.role] || [];

  const navigation = (
    <>
      <div className="sidebar-brand"><BrandLogo light /></div>
      <div className="sidebar-section-label">Workspace</div>
      <nav aria-label="Main navigation" className="sidebar-nav">
        <ul className="nav nav-pills flex-column gap-2">
          {navigationLinks.map((link) => (
            <li className="nav-item" key={link.path}>
              <NavLink
                to={link.path}
                end={link.end}
                className={({ isActive }) =>
                  `nav-link ${isActive ? "active" : "text-white"}`
                }
              >
                <i className={`bi bi-${link.icon}`} aria-hidden="true" />
                {link.label}
              </NavLink>
            </li>
          ))}
        </ul>
      </nav>
      <div className="sidebar-support mt-auto">
        <i className="bi bi-headset" aria-hidden="true" />
        <div><strong>Need help?</strong><small>Contact service support</small></div>
      </div>
    </>
  );

  return (
    <>
      <aside className="app-sidebar d-none d-lg-flex">{navigation}</aside>
      <aside className="offcanvas offcanvas-start app-sidebar mobile-sidebar" tabIndex="-1" id="mobileSidebar" aria-labelledby="mobileSidebarLabel">
        <div className="offcanvas-header">
          <span id="mobileSidebarLabel" className="visually-hidden">Main navigation</span>
          <button type="button" className="btn-close btn-close-white ms-auto" data-bs-dismiss="offcanvas" aria-label="Close navigation" />
        </div>
        <div className="offcanvas-body d-flex flex-column p-0">{navigation}</div>
      </aside>
    </>
  );
}

export default Sidebar;
