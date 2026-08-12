import { useEffect, useState } from "react";
import { Link, useParams } from "react-router";
import { getCustomerProfileByUserId } from "../../api/customerProfileApi";
import { getVehicleById } from "../../api/vehicleApi";
import LoadingSpinner from "../../components/shared/LoadingSpinner";
import { useAuth } from "../../context/AuthContext";

function DetailItem({ label, value }) {
  return (
    <div className="col-md-6">
      <p className="text-secondary mb-1">{label}</p>
      <p className="fw-semibold mb-0">{value || "Not provided"}</p>
    </div>
  );
}

function VehicleDetails() {
  const { id } = useParams();
  const { user } = useAuth();
  const [vehicle, setVehicle] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  useEffect(() => {
    async function loadVehicle() {
      try {
        setLoading(true);
        setError("");

        const [profileResponse, vehicleResponse] = await Promise.all([
          getCustomerProfileByUserId(user.userId),
          getVehicleById(id),
        ]);

        const customerProfile = profileResponse.data;
        const vehicleData = vehicleResponse.data;

        if (
          vehicleData.customerProfileId !==
          customerProfile.customerProfileId
        ) {
          setError("Vehicle not found.");
          return;
        }

        setVehicle(vehicleData);
      } catch (requestError) {
        setError(
          requestError.response?.status === 404
            ? "Vehicle not found."
            : "Could not load the vehicle."
        );
      } finally {
        setLoading(false);
      }
    }

    if (user?.userId && id) {
      loadVehicle();
    }
  }, [id, user?.userId]);

  if (loading) {
    return <LoadingSpinner message="Loading vehicle details..." />;
  }

  if (error) {
    return (
      <section>
        <div className="alert alert-danger" role="alert">
          {error}
        </div>
        <Link className="btn btn-outline-secondary" to="/customer/vehicles">
          Back to Vehicles
        </Link>
      </section>
    );
  }

  return (
    <section>
      <div className="d-flex justify-content-between align-items-center gap-3 mb-4">
        <div>
          <h2 className="mb-1">Vehicle Details</h2>
          <p className="text-secondary mb-0">
            {vehicle.make} {vehicle.model}
          </p>
        </div>

        <Link
          className="btn btn-primary"
          to={`/customer/vehicles/${vehicle.vehicleId}/edit`}
        >
          Edit Vehicle
        </Link>
      </div>

      <div className="card shadow-sm">
        <div className="card-body p-4">
          <div className="row g-4">
            <DetailItem label="Make" value={vehicle.make} />
            <DetailItem label="Model" value={vehicle.model} />
            <DetailItem label="Year" value={vehicle.year} />
            <DetailItem
              label="Plate Number"
              value={vehicle.plateNumber}
            />
            <DetailItem label="VIN" value={vehicle.vin} />
            <DetailItem label="Color" value={vehicle.color} />
            <DetailItem
              label="Mileage"
              value={
                vehicle.mileage == null
                  ? null
                  : Number(vehicle.mileage).toLocaleString()
              }
            />
          </div>
        </div>
      </div>

      <Link
        className="btn btn-outline-secondary mt-4"
        to="/customer/vehicles"
      >
        Back to Vehicles
      </Link>
    </section>
  );
}

export default VehicleDetails;
