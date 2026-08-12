import { useEffect, useState } from "react";
import {
  createCustomerProfile,
  deleteCustomerProfile,
  getCustomerProfileByUserId,
  updateCustomerProfile,
} from "../../api/customerProfileApi";
import LoadingSpinner from "../../components/shared/LoadingSpinner";
import { useAuth } from "../../context/AuthContext";

const emptyForm = {
  address: "",
  dateOfBirth: "",
};

function getErrorMessage(requestError) {
  const backendMessage = requestError.response?.data;

  if (typeof backendMessage === "string") {
    return backendMessage;
  }

  return "Could not save your customer profile.";
}

function CustomerProfile() {
  const { user } = useAuth();
  const [profile, setProfile] = useState(null);
  const [formData, setFormData] = useState(emptyForm);
  const [loading, setLoading] = useState(true);
  const [saving, setSaving] = useState(false);
  const [error, setError] = useState("");
  const [success, setSuccess] = useState("");

  useEffect(() => {
    async function loadProfile() {
      try {
        setLoading(true);
        setError("");

        const response = await getCustomerProfileByUserId(user.userId);
        const customerProfile = response.data;

        setProfile(customerProfile);
        setFormData({
          address: customerProfile.address ?? "",
          dateOfBirth: customerProfile.dateOfBirth ?? "",
        });
      } catch (requestError) {
        if (requestError.response?.status === 404) {
          setProfile(null);
          setFormData(emptyForm);
        } else {
          setError("Could not load your customer profile.");
        }
      } finally {
        setLoading(false);
      }
    }

    if (user?.userId) {
      loadProfile();
    }
  }, [user?.userId]);

  function handleChange(event) {
    const { name, value } = event.target;

    setFormData((currentData) => ({
      ...currentData,
      [name]: value,
    }));
    setError("");
    setSuccess("");
  }

  async function handleSubmit(event) {
    event.preventDefault();

    try {
      setSaving(true);
      setError("");
      setSuccess("");

      const profileData = {
        userId: user.userId,
        address: formData.address.trim() || null,
        dateOfBirth: formData.dateOfBirth || null,
      };

      if (profile) {
        await updateCustomerProfile(
          profile.customerProfileId,
          profileData
        );
        setProfile((currentProfile) => ({
          ...currentProfile,
          ...profileData,
        }));
        setSuccess("Your profile was updated successfully.");
      } else {
        const response = await createCustomerProfile(profileData);

        setProfile({
          customerProfileId: response.data.customerProfileId,
          ...profileData,
        });
        setSuccess("Your profile was created successfully.");
      }
    } catch (requestError) {
      setError(getErrorMessage(requestError));
    } finally {
      setSaving(false);
    }
  }

  async function handleDelete() {
    if (!profile || !window.confirm("Delete your customer profile? This cannot be undone.")) {
      return;
    }

    try {
      setSaving(true);
      setError("");
      setSuccess("");
      await deleteCustomerProfile(profile.customerProfileId);
      setProfile(null);
      setFormData(emptyForm);
      setSuccess("Your customer profile was deleted.");
    } catch (requestError) {
      setError(getErrorMessage(requestError));
    } finally {
      setSaving(false);
    }
  }

  function resetForm() {
    setFormData({
      address: profile?.address ?? "",
      dateOfBirth: profile?.dateOfBirth ?? "",
    });
    setError("");
    setSuccess("");
  }

  if (loading) {
    return <LoadingSpinner message="Loading your profile..." />;
  }

  const today = new Date().toISOString().split("T")[0];

  return (
    <section>
      <div className="mb-4">
        <h2 className="mb-1">Customer Profile</h2>
        <p className="text-secondary mb-0">
          View your account and manage your personal information.
        </p>
      </div>

      <div className="card shadow-sm">
        <div className="card-body p-4">
          <div className="row g-3 mb-4">
            <div className="col-md-6">
              <label className="form-label" htmlFor="customer-name">
                Name
              </label>
              <input
                id="customer-name"
                className="form-control"
                type="text"
                value={user?.userName ?? ""}
                disabled
              />
            </div>

            <div className="col-md-6">
              <label className="form-label" htmlFor="customer-role">
                Account Type
              </label>
              <input
                id="customer-role"
                className="form-control"
                type="text"
                value={user?.role ?? "Customer"}
                disabled
              />
            </div>
          </div>

          {error && (
            <div className="alert alert-danger" role="alert">
              {error}
            </div>
          )}

          {success && (
            <div className="alert alert-success" role="status">
              {success}
            </div>
          )}

          {!profile && (
            <div className="alert alert-info" role="alert">
              You do not have a customer profile yet. Complete the form below
              to create one.
            </div>
          )}

          <form onSubmit={handleSubmit}>
            <div className="mb-3">
              <label className="form-label" htmlFor="customer-address">
                Address
              </label>
              <textarea
                id="customer-address"
                className="form-control"
                name="address"
                value={formData.address}
                onChange={handleChange}
                maxLength={255}
                rows={3}
                disabled={saving}
                placeholder="Enter your address"
              />
              <div className="form-text">
                {formData.address.length}/255 characters
              </div>
            </div>

            <div className="mb-4">
              <label className="form-label" htmlFor="customer-date-of-birth">
                Date of Birth
              </label>
              <input
                id="customer-date-of-birth"
                className="form-control"
                type="date"
                name="dateOfBirth"
                value={formData.dateOfBirth}
                onChange={handleChange}
                max={today}
                disabled={saving}
              />
            </div>

            <div className="d-flex gap-2">
              <button
                className="btn btn-primary"
                type="submit"
                disabled={saving}
              >
                {saving
                  ? "Saving..."
                  : profile
                    ? "Update Profile"
                    : "Create Profile"}
              </button>

              {profile && (
                <button
                  className="btn btn-outline-secondary"
                  type="button"
                  onClick={resetForm}
                  disabled={saving}
                >
                  Reset
                </button>
              )}

              {profile && (
                <button
                  className="btn btn-outline-danger ms-auto"
                  type="button"
                  onClick={handleDelete}
                  disabled={saving}
                >
                  Delete Profile
                </button>
              )}
            </div>
          </form>
        </div>
      </div>
    </section>
  );
}

export default CustomerProfile;
