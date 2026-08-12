import { useEffect, useState } from "react";
import { Link } from "react-router";
import { getCustomerProfileByUserId } from "../../api/customerProfileApi";
import { deleteVehicle, getAllVehicles } from "../../api/vehicleApi";
import ConfirmationModal from "../../components/shared/ConfirmationModal";
import EmptyDataMessage from "../../components/shared/EmptyDataMessage";
import LoadingSpinner from "../../components/shared/LoadingSpinner";
import { useAuth } from "../../context/AuthContext";

function getErrorMessage(requestError, fallbackMessage) {
  const backendMessage = requestError.response?.data;

  return typeof backendMessage === "string"
    ? backendMessage
    : fallbackMessage;
}

function VehicleList() {
  const { user } = useAuth();
  const [profile, setProfile] = useState(null);
  const [vehicles, setVehicles] = useState([]);
  const [selectedVehicle, setSelectedVehicle] = useState(null);
  const [deletingId, setDeletingId] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  useEffect(() => {
    async function loadVehicles() {
      try {
        setLoading(true);
        setError("");

        const profileResponse = await getCustomerProfileByUserId(
          user.userId
        );
        const customerProfile = profileResponse.data;
        const vehiclesResponse = await getAllVehicles();

        setProfile(customerProfile);
        setVehicles(
          vehiclesResponse.data.filter(
            (vehicle) =>
              vehicle.customerProfileId ===
              customerProfile.customerProfileId
          )
        );
      } catch (requestError) {
        if (requestError.response?.status === 404) {
          setProfile(null);
          setVehicles([]);
        } else {
          setError("Could not load your vehicles.");
        }
      } finally {
        setLoading(false);
      }
    }

    if (user?.userId) {
      loadVehicles();
    }
  }, [user?.userId]);

  async function handleDelete() {
    if (!selectedVehicle) {
      return;
    }

    const vehicleId = selectedVehicle.vehicleId;
    setSelectedVehicle(null);

    try {
      setDeletingId(vehicleId);
      setError("");
      await deleteVehicle(vehicleId);
      setVehicles((currentVehicles) =>
        currentVehicles.filter(
          (vehicle) => vehicle.vehicleId !== vehicleId
        )
      );
    } catch (requestError) {
      setError(
        getErrorMessage(requestError, "Could not delete the vehicle.")
      );
    } finally {
      setDeletingId(null);
    }
  }

  if (loading) {
    return <LoadingSpinner message="Loading your vehicles..." />;
  }

  return (
    <section>
      <div className="d-flex justify-content-between align-items-center gap-3 mb-4">
        <div>
          <h2 className="mb-1">My Vehicles</h2>
          <p className="text-secondary mb-0">
            View and manage your registered vehicles.
          </p>
        </div>

        {profile && (
          <Link className="btn btn-primary" to="/customer/vehicles/new">
            Add Vehicle
          </Link>
        )}
      </div>

      {error && (
        <div className="alert alert-danger" role="alert">
          {error}
        </div>
      )}

      {!profile ? (
        <div className="card shadow-sm">
          <EmptyDataMessage
            title="Customer profile required"
            message="Create your customer profile before adding a vehicle."
          />
          <div className="text-center pb-4">
            <Link className="btn btn-primary" to="/customer/profile">
              Create Profile
            </Link>
          </div>
        </div>
      ) : vehicles.length === 0 ? (
        <div className="card shadow-sm">
          <EmptyDataMessage
            title="No vehicles registered"
            message="Add your first vehicle to get started."
          />
          <div className="text-center pb-4">
            <Link className="btn btn-primary" to="/customer/vehicles/new">
              Add Vehicle
            </Link>
          </div>
        </div>
      ) : (
        <div className="card shadow-sm">
          <div className="table-responsive">
            <table className="table table-hover align-middle mb-0">
              <thead className="table-light">
                <tr>
                  <th>Vehicle</th>
                  <th>Year</th>
                  <th>Plate Number</th>
                  <th>Color</th>
                  <th>Mileage</th>
                  <th className="text-end">Actions</th>
                </tr>
              </thead>
              <tbody>
                {vehicles.map((vehicle) => (
                  <tr key={vehicle.vehicleId}>
                    <td>
                      <span className="fw-semibold">{vehicle.make}</span>{" "}
                      {vehicle.model}
                    </td>
                    <td>{vehicle.year}</td>
                    <td>{vehicle.plateNumber}</td>
                    <td>{vehicle.color || "Not provided"}</td>
                    <td>
                      {vehicle.mileage == null
                        ? "Not provided"
                        : Number(vehicle.mileage).toLocaleString()}
                    </td>
                    <td>
                      <div className="d-flex justify-content-end gap-2">
                        <Link
                          className="btn btn-outline-primary btn-sm"
                          to={`/customer/vehicles/${vehicle.vehicleId}`}
                        >
                          View
                        </Link>
                        <Link
                          className="btn btn-outline-secondary btn-sm"
                          to={`/customer/vehicles/${vehicle.vehicleId}/edit`}
                        >
                          Edit
                        </Link>
                        <button
                          className="btn btn-outline-danger btn-sm"
                          type="button"
                          onClick={() => setSelectedVehicle(vehicle)}
                          disabled={deletingId === vehicle.vehicleId}
                        >
                          {deletingId === vehicle.vehicleId
                            ? "Deleting..."
                            : "Delete"}
                        </button>
                      </div>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        </div>
      )}

      <ConfirmationModal
        show={Boolean(selectedVehicle)}
        title="Delete Vehicle"
        message={
          selectedVehicle
            ? `Delete ${selectedVehicle.make} ${selectedVehicle.model} (${selectedVehicle.plateNumber})?`
            : "Delete this vehicle?"
        }
        confirmLabel="Delete"
        onConfirm={handleDelete}
        onCancel={() => setSelectedVehicle(null)}
      />
    </section>
  );
}

export default VehicleList;
