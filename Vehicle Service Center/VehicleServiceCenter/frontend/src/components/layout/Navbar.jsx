import { useNavigate } from "react-router";
import { useAuth } from "../../context/AuthContext";

function Navbar() {
  const { user, logout } = useAuth();
  const navigate = useNavigate();

  function handleLogout() {
    logout();
    navigate("/login", { replace: true });
  }

  return (
    <header className="navbar">
      <div>
        <p>Welcome, {user?.userName}</p>
        <small>{user?.role}</small>
      </div>

      <button type="button" onClick={handleLogout}>
        Logout
      </button>
    </header>
  );
}

export default Navbar;
