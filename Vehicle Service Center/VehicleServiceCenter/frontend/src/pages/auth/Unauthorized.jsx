import { Link } from "react-router";
import { useAuth } from "../../context/AuthContext";

function Unauthorized() {
  const { user } = useAuth();
  const message = sessionStorage.getItem("forbiddenMessage") ||
    "Your account does not have permission to access this page.";

  return (
    <main className="container py-5">
      <div className="card border-warning content-card-sm mx-auto">
        <div className="card-body p-5 text-center">
          <span className="badge text-bg-warning mb-3">403 Forbidden</span>
          <h1 className="h3">Access denied</h1>
          <p className="text-secondary mb-4">{message}</p>
          <Link className="btn btn-primary" to={user ? "/" : "/login"}>
            Return to dashboard
          </Link>
        </div>
      </div>
    </main>
  );
}

export default Unauthorized;
