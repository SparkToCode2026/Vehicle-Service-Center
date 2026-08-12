import { useEffect, useState } from "react";
import {
    getMechanicProfileByUserId,
    updateMechanicAvailability,
} from "../../api/mechanicProfileApi";
import { useAuth } from "../../context/AuthContext";

function MechanicAvailability() {
    const { user } = useAuth();

    const [mechanicProfile, setMechanicProfile] = useState(null);
    const [loading, setLoading] = useState(true);
    const [updating, setUpdating] = useState(false);
    const [error, setError] = useState("");
    const [successMessage, setSuccessMessage] = useState("");

    useEffect(() => {
        async function loadMechanicProfile() {
            try {
                setLoading(true);
                setError("");

                const response =
                    await getMechanicProfileByUserId(user.userId);

                setMechanicProfile(response.data);
            } catch (err) {
                console.error("Failed to load mechanic profile:", err);

                setError(
                    err.response?.data ||
                    "Failed to load mechanic profile."
                );
            } finally {
                setLoading(false);
            }
        }

        if (user?.userId) {
            loadMechanicProfile();
        }
    }, [user]);

    async function handleAvailabilityChange() {
        if (!mechanicProfile) {
            return;
        }

        try {
            setUpdating(true);
            setError("");
            setSuccessMessage("");

            const newAvailability =
                !mechanicProfile.isAvailable;

            await updateMechanicAvailability(
                mechanicProfile.mechanicProfileId,
                newAvailability
            );

            setMechanicProfile({
                ...mechanicProfile,
                isAvailable: newAvailability,
            });

            setSuccessMessage(
                "Availability updated successfully."
            );
        } catch (err) {
            console.error(
                "Failed to update availability:",
                err
            );

            setError(
                err.response?.data ||
                "Failed to update availability."
            );
        } finally {
            setUpdating(false);
        }
    }

    if (loading) {
        return (
            <div className="container mt-5">
                <p>Loading availability...</p>
            </div>
        );
    }

    if (error && !mechanicProfile) {
        return (
            <div className="container mt-5">
                <div className="alert alert-danger">
                    {error}
                </div>
            </div>
        );
    }

    return (
        <div className="container mt-5">
            <h2 className="mb-4">Mechanic Availability</h2>

            {error && (
                <div className="alert alert-danger">
                    {error}
                </div>
            )}

            {successMessage && (
                <div className="alert alert-success">
                    {successMessage}
                </div>
            )}

            <div className="card">
                <div className="card-body">
                    <h5 className="card-title">
                        Availability Status
                    </h5>

                    <p className="card-text">
                        Current status:
                        {" "}
                        <strong>
                            {mechanicProfile?.isAvailable
                                ? "Available"
                                : "Not Available"}
                        </strong>
                    </p>

                    <button
                        type="button"
                        className={
                            mechanicProfile?.isAvailable
                                ? "btn btn-danger"
                                : "btn btn-success"
                        }
                        onClick={handleAvailabilityChange}
                        disabled={updating}
                    >
                        {updating
                            ? "Updating..."
                            : mechanicProfile?.isAvailable
                                ? "Set as Not Available"
                                : "Set as Available"}
                    </button>
                </div>
            </div>
        </div>
    );
}

export default MechanicAvailability;