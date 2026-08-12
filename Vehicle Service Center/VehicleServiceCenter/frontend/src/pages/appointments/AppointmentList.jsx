import { useEffect, useState } from "react";
import { Link } from "react-router";
import { getAppointments } from "../../api/appointmentApi";
import { getCustomerProfileByUserId } from "../../api/customerProfileApi";
import { useAuth } from "../../context/AuthContext";

function AppointmentList() {
    const { user } = useAuth();

    const [appointments, setAppointments] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState("");

    useEffect(() => {
        async function loadAppointments() {
            try {
                setLoading(true);
                setError("");

                // Get the logged-in customer's profile
                const customerResponse =
                    await getCustomerProfileByUserId(user.userId);

                const customerProfileId =
                    customerResponse.data.customerProfileId;

                // Get all appointments from the backend
                const data = await getAppointments();

                // Show only this customer's appointments
                const myAppointments = data.filter(
                    (appointment) =>
                        appointment.customerProfileId === customerProfileId
                );

                setAppointments(myAppointments);
            } catch (err) {
                console.error(
                    "Failed to load appointments:",
                    err
                );

                setError("Failed to load appointments.");
            } finally {
                setLoading(false);
            }
        }

        if (user?.userId) {
            loadAppointments();
        }
    }, [user]);

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
                            <th>Action</th>
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

                                <td>
                                    {appointment.notes || "—"}
                                </td>
                                <td><Link className="btn btn-outline-primary btn-sm" to={`/appointments/${appointment.appointmentId}`}>Details</Link></td>
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
