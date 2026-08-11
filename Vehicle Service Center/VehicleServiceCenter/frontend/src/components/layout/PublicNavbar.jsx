import { NavLink, Link } from "react-router";
function PublicNavbar() {
    return (
        <nav className="navbar navbar-expand-lg navbar-light bg-white border-bottom shadow-sm sticky-top">
            <div className="container">
                <Link className="navbar-brand fw-bold" to="/">
                    Vehicle Service Center
                </Link>

                <button
                    className="navbar-toggler"
                    type="button"
                    data-bs-toggle="collapse"
                    data-bs-target="#publicNavbarNav"
                    aria-controls="publicNavbarNav"
                    aria-expanded="false"
                    aria-label="Toggle navigation"
                >
                    <span className="navbar-toggler-icon"></span>
                </button>

                <div className="collapse navbar-collapse" id="publicNavbarNav">
                    <ul className="navbar-nav ms-auto gap-2">
                        <li className="nav-item">
                            <NavLink className="nav-link" to="/" end>
                                Home
                            </NavLink>
                        </li>
                        <li className="nav-item">
                            <NavLink className="nav-link" to="/about">
                                About
                            </NavLink>
                        </li>
                        <li className="nav-item">
                            <NavLink className="nav-link" to="/services">
                                Services
                            </NavLink>
                        </li>
                        <li className="nav-item">
                            <NavLink className="nav-link" to="/branches">
                                Branches
                            </NavLink>
                        </li>
                        <li className="nav-item ms-lg-2">
                            <Link className="btn btn-primary btn-sm" to="/login">
                                Login
                            </Link>
                        </li>
                    </ul>
                </div>
            </div>
        </nav>
    );
}

export default PublicNavbar;