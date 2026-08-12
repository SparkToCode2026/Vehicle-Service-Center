import { Outlet, NavLink, Link } from "react-router";
import { useAuth } from "../../context/AuthContext";
import Navbar from "./Navbar";
import Sidebar from "./Sidebar";

function AppLayout() {
  const { user } = useAuth();

  if (!user) {
    return (
        <div className="d-flex flex-column min-vh-100">
          <nav className="navbar navbar-expand-lg navbar-light bg-white border-bottom sticky-top">
            <div className="container">
              <Link className="navbar-brand fw-bold" to="/">
                Vehicle Service Center
              </Link>

              <button
                  className="navbar-toggler"
                  type="button"
                  data-bs-toggle="collapse"
                  data-bs-target="#navMenu"
              >
                <span className="navbar-toggler-icon"></span>
              </button>

              <div className="collapse navbar-collapse" id="navMenu">
                <ul className="navbar-nav ms-auto gap-2">
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
              </div>
            </div>
          </nav>

          <main className="flex-grow-1">
            <Outlet />
          </main>
        </div>
    );
  }

  return (
      <div className="container-fluid min-vh-100 bg-light">
        <div className="row min-vh-100">
          <Sidebar />

          <div className="col p-0 d-flex flex-column">
            <Navbar />

            <main className="flex-grow-1 p-4">
              <Outlet />
            </main>
          </div>
        </div>
      </div>
  );
}

export default AppLayout;