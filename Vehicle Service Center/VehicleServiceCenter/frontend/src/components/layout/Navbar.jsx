import { Link, useNavigate } from "react-router";
import { useAuth } from "../../context/AuthContext";

function Navbar() {
  const { user, logout } = useAuth();
  const navigate = useNavigate();

  function handleLogout() {
    logout();
    navigate("/login", { replace: true });
  }

  return (
    <header className="navbar bg-white border-bottom px-4 py-3">
      <div>
        <span className="me-2">Welcome, {user?.userName}</span>
        <span className="badge text-bg-secondary">
          {user?.role}
        </span>
      </div>

      <div className="d-flex gap-2">
        <Link className="btn btn-outline-secondary btn-sm" to="/account">Account</Link>
        <button type="button" className="btn btn-outline-danger btn-sm" onClick={handleLogout}>Logout</button>
      </div>
    </header>
  );
}

export default Navbar;
