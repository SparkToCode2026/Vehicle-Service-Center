import { useEffect, useState } from "react";
import {
    getAppointments,
    updateAppointmentStatus,
    deleteAppointment,
} from "../../api/appointmentApi";

function AppointmentManagement() {
    const [appointments, setAppointments] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState("");
    const [updatingId, setUpdatingId] = useState(null);

    useEffect(() => {
        loadAppointments();
    }, []);

    async function loadAppointments() {
        try {
            setLoading(true);
            setError("");

            const data = await getAppointments();

            setAppointments(data);
        } catch (err) {
            console.error("Failed to load appointments:", err);

            setError(
                err.response?.data ||
                "Failed to load appointments."
            );
        } finally {
            setLoading(false);
        }
    }

    async function handleStatusChange(appointmentId, status) {
        try {
            setUpdatingId(appointmentId);
            setError("");

            await updateAppointmentStatus(
                appointmentId,
                status
            );

            await loadAppointments();
        } catch (err) {
            console.error(
                "Failed to update appointment status:",
                err
            );

            setError(
                err.response?.data ||
                "Failed to update appointment status."
            );
        } finally {
            setUpdatingId(null);
        }
    }

    async function handleDelete(appointmentId) {
        const confirmed = window.confirm(
            "Are you sure you want to delete this appointment?"
        );

        if (!confirmed) {
            return;
        }

        try {
            setError("");

            await deleteAppointment(appointmentId);

            setAppointments((currentAppointments) =>
                currentAppointments.filter(
                    (appointment) =>
                        appointment.appointmentId !== appointmentId
                )
            );
        } catch (err) {
            console.error(
                "Failed to delete appointment:",
                err
            );

            setError(
                err.response?.data ||
                "Failed to delete appointment."
            );
        }
    }

    function getStatusClass(status) {
        switch (status?.toLowerCase()) {
            case "confirmed":
                return "bg-success";

            case "pending":
                return "bg-warning text-dark";

            case "completed":
                return "bg-primary";

            case "cancelled":
                return "bg-danger";

            case "in progress":
                return "bg-info text-dark";

            default:
                return "bg-secondary";
        }
    }

    if (loading) {
        return (
            <div className="container mt-5">
                <p>Loading appointments...</p>
            </div>
        );
    }

    return (
        <div className="container mt-5">

            <div className="d-flex justify-content-between align-items-center mb-4">
                <h2>Appointment Management</h2>

                <button
                    className="btn btn-outline-primary"
                    onClick={loadAppointments}
                >
                    Refresh
                </button>
            </div>

            {error && (
                <div className="alert alert-danger">
                    {error}
                </div>
            )}

            {appointments.length === 0 ? (
                <div className="alert alert-info">
                    No appointments found.
                </div>
            ) : (
                <div className="table-responsive">

                    <table className="table table-bordered table-striped align-middle">

                        <thead>
                        <tr>
                            <th>Customer</th>
                            <th>Vehicle</th>
                            <th>Service</th>
                            <th>Branch</th>
                            <th>Mechanic</th>
                            <th>Date</th>
                            <th>Status</th>
                            <th>Notes</th>
                            <th>Actions</th>
                        </tr>
                        </thead>

                        <tbody>

                        {appointments.map((appointment) => (
                            <tr key={appointment.appointmentId}>

                                {/* Customer */}
                                <td>
                                    {appointment.customerProfile?.user?.userName ||
                                        appointment.customerProfile?.userName ||
                                        `Customer #${appointment.customerProfileId}`}
                                </td>

                                {/* Vehicle */}
                                <td>
                                    {appointment.vehicle?.make}{" "}
                                    {appointment.vehicle?.model}
                                    <br />
                                    <small className="text-muted">
                                        {appointment.vehicle?.plateNumber}
                                    </small>
                                </td>

                                {/* Service */}
                                <td>
                                    {appointment.serviceType?.name ||
                                        appointment.serviceType?.serviceName ||
                                        "N/A"}
                                </td>

                                {/* Branch */}
                                <td>
                                    {appointment.branch?.name ||
                                        appointment.branch?.branchName ||
                                        "N/A"}
                                </td>

                                {/* Mechanic */}
                                <td>
                                    {appointment.mechanicProfile?.user?.userName ||
                                        appointment.mechanicProfile?.userName ||
                                        appointment.mechanicProfileId ||
                                        "Not assigned"}
                                </td>

                                {/* Date */}
                                <td>
                                    {new Date(
                                        appointment.appointmentDate
                                    ).toLocaleString()}
                                </td>

                                {/* Status */}
                                <td>
                                        <span
                                            className={`badge ${getStatusClass(
                                                appointment.status
                                            )}`}
                                        >
                                            {appointment.status}
                                        </span>
                                </td>

                                {/* Notes */}
                                <td>
                                    {appointment.notes || "—"}
                                </td>

                                {/* Actions */}
                                <td>

                                    <select
                                        className="form-select form-select-sm mb-2"
                                        value={
                                            appointment.status || ""
                                        }
                                        disabled={
                                            updatingId ===
                                            appointment.appointmentId
                                        }
                                        onChange={(event) =>
                                            handleStatusChange(
                                                appointment.appointmentId,
                                                event.target.value
                                            )
                                        }
                                    >
                                        <option value="Pending">
                                            Pending
                                        </option>

                                        <option value="Confirmed">
                                            Confirmed
                                        </option>

                                        <option value="In Progress">
                                            In Progress
                                        </option>

                                        <option value="Completed">
                                            Completed
                                        </option>

                                        <option value="Cancelled">
                                            Cancelled
                                        </option>
                                    </select>

                                    <button
                                        className="btn btn-sm btn-danger"
                                        onClick={() =>
                                            handleDelete(
                                                appointment.appointmentId
                                            )
                                        }
                                    >
                                        Delete
                                    </button>

                                </td>

                            </tr>
                        ))}

                        </tbody>

                    </table>

                </div>
            )}

        </div>
    );
}

export default AppointmentManagement;