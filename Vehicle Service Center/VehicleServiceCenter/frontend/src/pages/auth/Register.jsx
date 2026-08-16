import { useState } from "react";
import { Link, Navigate } from "react-router";
import { useAuth } from "../../context/AuthContext";

function Register() {
  const { isAuthenticated, register } = useAuth();

  const [formData, setFormData] = useState({
    userName: "",
    email: "",
    phoneNumber: "",
    password: "",
    confirmPassword: "",
  });

  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");
  const [success, setSuccess] = useState("");

  if (isAuthenticated) {
    return <Navigate to="/" replace />;
  }

  function handleChange(event) {
    const { name, value } = event.target;

    setFormData((currentData) => ({
      ...currentData,
      [name]: value,
    }));
  }

  async function handleSubmit(event) {
    event.preventDefault();

    if (formData.password !== formData.confirmPassword) {
      setError("The passwords do not match.");
      return;
    }

    try {
      setLoading(true);
      setError("");
      setSuccess("");

      await register({
        userName: formData.userName.trim(),
        email: formData.email.trim(),
        phoneNumber: formData.phoneNumber.trim() || null,
        password: formData.password,
        role: "Customer",
      });

      setSuccess("Registration successful. You can now log in.");

      setFormData({
        userName: "",
        email: "",
        phoneNumber: "",
        password: "",
        confirmPassword: "",
      });
    } catch (requestError) {
      const backendMessage = requestError.response?.data;

      setError(
        typeof backendMessage === "string"
          ? backendMessage
          : "Registration failed. Please check your information."
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
                <h1 className="h3 mb-2">Create an Account</h1>
                <p className="text-secondary mb-0">
                  Register as a Vehicle Service Center customer
                </p>
              </div>

              <div className="alert alert-info" role="alert">
                Public registration creates Customer accounts only.
              </div>

              {error && (
                <div className="alert alert-danger" role="alert">
                  {error}
                </div>
              )}

              {success && (
                <div className="alert alert-success" role="alert">
                  {success}
                </div>
              )}

              <form onSubmit={handleSubmit}>
                <div className="mb-3">
                  <label className="form-label" htmlFor="userName">
                    Full name
                  </label>
                  <input
                    id="userName"
                    name="userName"
                    className="form-control"
                    type="text"
                    value={formData.userName}
                    onChange={handleChange}
                    autoComplete="name"
                    required
                  />
                </div>

                <div className="mb-3">
                  <label className="form-label" htmlFor="email">
                    Email address
                  </label>
                  <input
                    id="email"
                    name="email"
                    className="form-control"
                    type="email"
                    value={formData.email}
                    onChange={handleChange}
                    autoComplete="email"
                    required
                  />
                </div>

                <div className="mb-3">
                  <label className="form-label" htmlFor="phoneNumber">
                    Phone number
                  </label>
                  <input
                    id="phoneNumber"
                    name="phoneNumber"
                    className="form-control"
                    type="tel"
                    value={formData.phoneNumber}
                    onChange={handleChange}
                    autoComplete="tel"
                  />
                </div>

                <div className="row">
                  <div className="col-md-6 mb-3">
                    <label className="form-label" htmlFor="password">
                      Password
                    </label>
                    <input
                      id="password"
                      name="password"
                      className="form-control"
                      type="password"
                      value={formData.password}
                      onChange={handleChange}
                      autoComplete="new-password"
                      minLength="6"
                      required
                    />
                  </div>

                  <div className="col-md-6 mb-4">
                    <label className="form-label" htmlFor="confirmPassword">
                      Confirm password
                    </label>
                    <input
                      id="confirmPassword"
                      name="confirmPassword"
                      className="form-control"
                      type="password"
                      value={formData.confirmPassword}
                      onChange={handleChange}
                      autoComplete="new-password"
                      minLength="6"
                      required
                    />
                  </div>
                </div>

                <button
                  className="btn btn-primary w-100 btn-icon-label justify-content-center"
                  type="submit"
                  disabled={loading}
                >
                  {loading ? <><span className="spinner-border spinner-border-sm" aria-hidden="true" /> Creating account...</> : <><i className="bi bi-person-plus" aria-hidden="true" /> Create Account</>}
                </button>
              </form>

              <p className="auth-footer-link mt-4 mb-0">
                Already have an account? <Link to="/login">Login</Link>
              </p>
            </div>
          </div>
    </main>
  );
}

export default Register;
