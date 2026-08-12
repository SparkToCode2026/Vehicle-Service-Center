import { useEffect, useState } from "react";
import { Link, useNavigate, useParams } from "react-router";
import { getCustomerProfileByUserId } from "../../api/customerProfileApi";
import {
  createVehicle,
  getVehicleById,
  updateVehicle,
} from "../../api/vehicleApi";
import VehicleForm from "../../components/customer/VehicleForm";
import LoadingSpinner from "../../components/shared/LoadingSpinner";
import { useAuth } from "../../context/AuthContext";

function getErrorMessage(requestError) {
  const responseData = requestError.response?.data;

  if (typeof responseData === "string") {
    return responseData;
  }

  if (responseData?.errors) {
    const validationMessages = Object.values(responseData.errors).flat();

    if (validationMessages.length > 0) {
      return validationMessages.join(" ");
    }
  }

  return "Could not save the vehicle.";
}

function VehicleFormPage() {
  const { id } = useParams();
  const navigate = useNavigate();
  const { user } = useAuth();
  const isEditing = Boolean(id);

  const [customerProfileId, setCustomerProfileId] = useState(null);
  const [vehicle, setVehicle] = useState(null);
  const [loading, setLoading] = useState(true);
  const [saving, setSaving] = useState(false);
  const [error, setError] = useState("");
  const [profileMissing, setProfileMissing] = useState(false);

  useEffect(() => {
    async function loadPage() {
      try {
        setLoading(true);
        setError("");
        setProfileMissing(false);

        const profileResponse = await getCustomerProfileByUserId(
          user.userId
        );
        const profile = profileResponse.data;

        setCustomerProfileId(profile.customerProfileId);

        if (!isEditing) {
          return;
        }

        const vehicleResponse = await getVehicleById(id);
        const vehicleData = vehicleResponse.data;

        if (vehicleData.customerProfileId !== profile.customerProfileId) {
          setError("Vehicle not found.");
          return;
        }

        setVehicle(vehicleData);
      } catch (requestError) {
        if (requestError.response?.status === 404 && !isEditing) {
          setProfileMissing(true);
        } else {
          setError(
            requestError.response?.status === 404
              ? "Vehicle or customer profile not found."
              : "Could not load the vehicle form."
          );
        }
      } finally {
        setLoading(false);
      }
    }

    if (user?.userId) {
      loadPage();
    }
  }, [id, isEditing, user?.userId]);

  async function handleSubmit(vehicleData) {
    try {
      setSaving(true);
      setError("");

      if (isEditing) {
        await updateVehicle(id, vehicleData);
        navigate(`/customer/vehicles/${id}`);
        return;
      }

      const response = await createVehicle({
        ...vehicleData,
        createdAt: new Date().toISOString(),
      });

      navigate(`/customer/vehicles/${response.data.vehicleId}`);
    } catch (requestError) {
      setError(getErrorMessage(requestError));
    } finally {
      setSaving(false);
    }
  }

  function handleCancel() {
    navigate(
      isEditing ? `/customer/vehicles/${id}` : "/customer/vehicles"
    );
  }

  if (loading) {
    return (
      <LoadingSpinner
        message={isEditing ? "Loading vehicle..." : "Loading form..."}
      />
    );
  }

  if (profileMissing) {
    return (
      <section>
        <div className="alert alert-info" role="alert">
          Create your customer profile before adding a vehicle.
        </div>
        <Link className="btn btn-primary" to="/customer/profile">
          Create Profile
        </Link>
      </section>
    );
  }

  if (error && (!customerProfileId || (isEditing && !vehicle))) {
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
      <div className="mb-4">
        <h2 className="mb-1">
          {isEditing ? "Edit Vehicle" : "Add Vehicle"}
        </h2>
        <p className="text-secondary mb-0">
          {isEditing
            ? "Update your vehicle information."
            : "Register a vehicle in your account."}
        </p>
      </div>

      <div className="card shadow-sm">
        <div className="card-body p-4">
          <VehicleForm
            initialValues={vehicle ?? undefined}
            customerProfileId={customerProfileId}
            onSubmit={handleSubmit}
            onCancel={handleCancel}
            submitLabel={isEditing ? "Update Vehicle" : "Add Vehicle"}
            isSubmitting={saving}
            error={error}
          />
        </div>
      </div>
    </section>
  );
}

export default VehicleFormPage;
