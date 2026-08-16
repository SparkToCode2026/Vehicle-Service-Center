import { Outlet, NavLink, Link } from "react-router";
import { useAuth } from "../../context/AuthContext";
import Navbar from "./Navbar";
import PublicFooter from "./PublicFooter";
import Sidebar from "./Sidebar";
import BrandLogo from "./BrandLogo";

function AppLayout() {
  const { user } = useAuth();

  if (!user) {
    return (
        <div className="public-shell d-flex flex-column min-vh-100">
          <nav className="public-navbar navbar navbar-expand-lg sticky-top" aria-label="Public navigation">
            <div className="container">
              <BrandLogo />

              <button
                  className="navbar-toggler"
                  type="button"
                  data-bs-toggle="collapse"
                  data-bs-target="#navMenu"
                  aria-controls="navMenu"
                  aria-expanded="false"
                  aria-label="Toggle navigation"
              >
                <span className="navbar-toggler-icon"></span>
              </button>

              <div className="collapse navbar-collapse" id="navMenu">
                <ul className="navbar-nav ms-auto gap-lg-1 align-items-lg-center">
                  <li className="nav-item">
                    <NavLink className="nav-link" to="/" end>Home</NavLink>
                  </li>
                  <li className="nav-item">
                    <NavLink className="nav-link" to="/about">About</NavLink>
                  </li>
                  <li className="nav-item">
                    <NavLink className="nav-link" to="/services">Services</NavLink>
                  </li>
                  <li className="nav-item">
                    <NavLink className="nav-link" to="/branches">Branches</NavLink>
                  </li>
                </ul>

                <div className="d-flex flex-column flex-lg-row gap-2 ms-lg-3 mt-3 mt-lg-0">
                  <Link className="btn btn-outline-primary btn-sm" to="/login">
                    Sign In
                  </Link>
                  <Link className="btn btn-accent btn-sm" to="/register">
                    Create Account
                  </Link>
                </div>
              </div>
            </div>
          </nav>

          <main className="flex-grow-1">
            <Outlet />
          </main>

          <PublicFooter />
        </div>
    );
  }

  return (
      <div className="app-shell container-fluid min-vh-100 p-0">
        <div className="d-flex min-vh-100">
          <Sidebar />

          <div className="app-column d-flex flex-column min-vh-100">
            <Navbar />

            <main className="app-main flex-grow-1">
              <div className="container-fluid app-content">
                <Outlet />
              </div>
            </main>
            <footer className="app-footer">
              <span>&copy; {new Date().getFullYear()} Vehicle Service Center</span>
              <span className="d-none d-sm-inline">Professional automotive care</span>
            </footer>
          </div>
        </div>
      </div>
  );
}

export default AppLayout;
