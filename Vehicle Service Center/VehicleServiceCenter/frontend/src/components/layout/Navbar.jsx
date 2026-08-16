import { Link, useNavigate } from "react-router";
import { useAuth } from "../../context/AuthContext";
import BrandLogo from "./BrandLogo";

function Navbar() {
  const { user, logout } = useAuth();
  const navigate = useNavigate();

  function handleLogout() {
    logout();
    navigate("/login", { replace: true });
  }

  return (
    <header className="app-navbar navbar sticky-top">
      <div className="d-flex align-items-center gap-2">
        <button
          className="btn mobile-menu-button d-lg-none"
          type="button"
          data-bs-toggle="offcanvas"
          data-bs-target="#mobileSidebar"
          aria-controls="mobileSidebar"
          aria-label="Open navigation menu"
        >
          <i className="bi bi-list" aria-hidden="true" />
        </button>
        <div className="d-lg-none"><BrandLogo compact /></div>
        <div className="welcome-copy d-none d-md-block">
          <span className="welcome-label">Welcome back</span>
          <strong>{user?.userName}</strong>
        </div>
      </div>

      <div className="d-flex align-items-center gap-2">
        <span className="role-badge badge">
          <i className="bi bi-person-badge me-1" aria-hidden="true" />{user?.role}
        </span>
        <Link className="btn btn-icon-label btn-outline-secondary btn-sm" to="/account">
          <i className="bi bi-gear" aria-hidden="true" /><span className="d-none d-sm-inline">Account</span>
        </Link>
        <button type="button" className="btn btn-icon-label btn-outline-danger btn-sm" onClick={handleLogout}>
          <i className="bi bi-box-arrow-right" aria-hidden="true" /><span className="d-none d-sm-inline">Logout</span>
        </button>
      </div>
    </header>
  );
}

export default Navbar;
