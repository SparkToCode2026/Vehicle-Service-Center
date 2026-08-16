import { useState } from "react";
import { Link, Navigate, useSearchParams } from "react-router";
import { useAuth } from "../../context/AuthContext";
import { getHomePageForRole } from "../../utils/roleAccess";

function Login() {
  const { user, isAuthenticated, login } = useAuth();
  const [searchParams] = useSearchParams();

  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");

  if (isAuthenticated) {
    return <Navigate to={getHomePageForRole(user?.role)} replace />;
  }

  async function handleSubmit(event) {
    event.preventDefault();

    try {
      setLoading(true);
      setError("");

      await login(email, password);
    } catch (requestError) {
      const backendMessage = requestError.response?.data;

      setError(
        typeof backendMessage === "string"
          ? backendMessage
          : "Login failed. Please check your email and password."
      );
    } finally {
      setLoading(false);
    }
  }

  return (
    <main className="auth-page">
          <div className="auth-card">
            <div className="auth-card-body">
              <Link className="auth-brand" to="/" aria-label="Back to home">
                <img className="auth-brand-image" src="/vehicle-service-center-logo.png" alt="Vehicle Service Center" />
              </Link>
              <div className="auth-title">
                <h1 className="h3 mb-2">Welcome Back</h1>
                <p className="text-secondary mb-0">
                  Sign in to your account
                </p>
              </div>

              {error && (
                <div className="alert alert-danger" role="alert">
                  {error}
                </div>
              )}

              {searchParams.get("reason") === "session-expired" && !error && (
                <div className="alert alert-warning" role="alert">
                  Your session expired. Please sign in again.
                </div>
              )}

              <form onSubmit={handleSubmit}>
                <div className="mb-3">
                  <label className="form-label" htmlFor="email">
                    Email address
                  </label>
                  <input
                    id="email"
                    className="form-control"
                    type="email"
                    value={email}
                    onChange={(event) => setEmail(event.target.value)}
                    autoComplete="email"
                    placeholder="name@example.com"
                    required
                  />
                </div>

                <div className="mb-4">
                  <label className="form-label" htmlFor="password">
                    Password
                  </label>
                  <input
                    id="password"
                    className="form-control"
                    type="password"
                    value={password}
                    onChange={(event) => setPassword(event.target.value)}
                    autoComplete="current-password"
                    required
                  />
                </div>

                <button
                  className="btn btn-primary w-100 btn-icon-label justify-content-center"
                  type="submit"
                  disabled={loading}
                >
                  {loading ? <><span className="spinner-border spinner-border-sm" aria-hidden="true" /> Signing in...</> : <><i className="bi bi-box-arrow-in-right" aria-hidden="true" /> Sign In</>}
                </button>
              </form>

              <p className="auth-footer-link mt-4 mb-0">
                Don&apos;t have an account?{" "}
                <Link to="/register">Register</Link>
              </p>
            </div>
          </div>
    </main>
  );
}

export default Login;
