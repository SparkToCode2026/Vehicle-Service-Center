import { useEffect, useState } from "react";
import { Link } from "react-router";
import { getCustomerProfileByUserId } from "../../api/customerProfileApi";
import { getAllVehicles } from "../../api/vehicleApi";
import LoadingSpinner from "../../components/shared/LoadingSpinner";
import { useAuth } from "../../context/AuthContext";

function CustomerDashboard() {
  const { user } = useAuth();
  const [profile, setProfile] = useState(null);
  const [vehicleCount, setVehicleCount] = useState(0);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  useEffect(() => {
    async function loadDashboard() {
      try {
        setLoading(true);
        setError("");

        const profileResponse = await getCustomerProfileByUserId(
          user.userId
        );
        const customerProfile = profileResponse.data;

        setProfile(customerProfile);

        const vehiclesResponse = await getAllVehicles();
        const customerVehicles = vehiclesResponse.data.filter(
          (vehicle) =>
            vehicle.customerProfileId ===
            customerProfile.customerProfileId
        );

        setVehicleCount(customerVehicles.length);
      } catch (requestError) {
        if (requestError.response?.status === 404) {
          setProfile(null);
          setVehicleCount(0);
        } else {
          setError("Could not load your dashboard information.");
        }
      } finally {
        setLoading(false);
      }
    }

    if (user?.userId) {
      loadDashboard();
    }
  }, [user?.userId]);

  if (loading) {
    return <LoadingSpinner message="Loading your dashboard..." />;
  }

  const profileIsComplete = Boolean(
    profile?.address && profile?.dateOfBirth
  );

  return (
    <section>
      <div className="page-header">
        <h2 className="mb-1">Customer Dashboard</h2>
        <p className="text-secondary mb-0">
          Welcome, {user?.userName}. Manage your profile, vehicles, and
          service orders.
        </p>
      </div>

      {error && (
        <div className="alert alert-danger" role="alert">
          {error}
        </div>
      )}

      {!profile && !error && (
        <div className="alert alert-info" role="alert">
          <h3 className="h5">Complete your customer profile</h3>
          <p className="mb-3">
            Create your profile before adding and managing vehicles.
          </p>
          <Link className="btn btn-primary" to="/customer/profile">
            Create Profile
          </Link>
        </div>
      )}

      <div className="row g-3 mb-4">
        <div className="col-md-6">
          <div className="card dashboard-card h-100">
            <div className="card-body">
              <div className="feature-icon"><i className="bi bi-person-check" aria-hidden="true" /></div>
              <p className="text-secondary mb-2">Profile Status</p>
              <h3 className="h4 mb-3">
                {profileIsComplete ? "Complete" : "Needs Attention"}
              </h3>
              <Link
                className="btn btn-outline-primary"
                to="/customer/profile"
              >
                {profile ? "Manage Profile" : "Create Profile"}
              </Link>
            </div>
          </div>
        </div>

        <div className="col-md-6">
          <div className="card dashboard-card h-100">
            <div className="card-body">
              <div className="feature-icon"><i className="bi bi-car-front" aria-hidden="true" /></div>
              <p className="text-secondary mb-2">Registered Vehicles</p>
              <h3 className="h4 mb-3">{vehicleCount}</h3>
              <Link
                className="btn btn-outline-primary"
                to={profile ? "/customer/vehicles" : "/customer/profile"}
              >
                {profile ? "View Vehicles" : "Complete Profile"}
              </Link>
            </div>
          </div>
        </div>
      </div>

      <div className="card dashboard-card">
        <div className="card-body d-flex justify-content-between align-items-center gap-3">
          <div>
            <h3 className="h5 mb-1">Service Orders</h3>
            <p className="text-secondary mb-0">
              View your vehicle service history and order status.
            </p>
          </div>
          <Link className="btn btn-primary" to="/service-orders">
            View Service Orders
          </Link>
        </div>
      </div>
    </section>
  );
}

export default CustomerDashboard;
