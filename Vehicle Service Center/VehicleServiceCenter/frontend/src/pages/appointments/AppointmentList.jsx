import { useEffect, useState } from "react";
import { getAppointments } from "../../api/appointmentApi";

function AppointmentList() {
    const [appointments, setAppointments] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState("");

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
            setError("Failed to load appointments.");
        } finally {
            setLoading(false);
        }
    }

    if (loading) {
        return (
            <div className="container mt-5">
                <p>Loading appointments...</p>
            </div>
        );
    }

    if (error) {
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
            <h2 className="mb-4">My Appointments</h2>

            {appointments.length === 0 ? (
                <div className="alert alert-info">
                    You don't have any appointments yet.
                </div>
            ) : (
                <div className="table-responsive">
                    <table className="table table-bordered table-striped">
                        <thead>
                        <tr>
                            <th>Vehicle</th>
                            <th>Service</th>
                            <th>Branch</th>
                            <th>Date</th>
                            <th>Status</th>
                            <th>Notes</th>
                        </tr>
                        </thead>

                        <tbody>
                        {appointments.map((appointment) => (
                            <tr key={appointment.appointmentId}>
                                <td>
                                    {appointment.vehicle?.make}{" "}
                                    {appointment.vehicle?.model}
                                </td>

                                <td>
                                    {appointment.serviceType?.name ||
                                        appointment.serviceType?.serviceName ||
                                        "N/A"}
                                </td>

                                <td>
                                    {appointment.branch?.name ||
                                        appointment.branch?.branchName ||
                                        "N/A"}
                                </td>

                                <td>
                                    {new Date(
                                        appointment.appointmentDate
                                    ).toLocaleString()}
                                </td>

                                <td>
                    <span className="badge bg-primary">
                      {appointment.status}
                    </span>
                                </td>

                                <td>{appointment.notes || "—"}</td>
                            </tr>
                        ))}
                        </tbody>
                    </table>
                </div>
            )}
        </div>
    );
}

export default AppointmentList;