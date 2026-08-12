import { Link } from "react-router";
import { useAuth } from "../../context/AuthContext";

function MechanicDashboard() {
    const { user } = useAuth();

    return (
        <section>
            <div className="mb-4">
                <h2 className="mb-1">Mechanic Dashboard</h2>
                <p className="text-secondary mb-0">
                    Welcome, {user?.userName}. Manage your availability and assigned
                    service work.
                </p>
            </div>

            <div className="row g-3">
                <div className="col-md-6">
                    <div className="card h-100 shadow-sm">
                        <div className="card-body">
                            <h3 className="h5">Availability</h3>
                            <p className="text-secondary">
                                Update your current availability status.
                            </p>

                            <Link
                                className="btn btn-primary"
                                to="/mechanic/availability"
                            >
                                Manage Availability
                            </Link>
                        </div>
                    </div>
                </div>

                <div className="col-md-6">
                    <div className="card h-100 shadow-sm">
                        <div className="card-body">
                            <h3 className="h5">Assigned Service Orders</h3>
                            <p className="text-secondary">
                                View the service orders assigned to you.
                            </p>

                            <Link
                                className="btn btn-primary"
                                to="/service-orders"
                            >
                                View Assigned Work
                            </Link>
                        </div>
                    </div>
                </div>

                <div className="col-md-6">
                    <div className="card h-100 shadow-sm">
                        <div className="card-body">
                            <h3 className="h5">Appointment Management</h3>
                            <p className="text-secondary">
                                View and manage customer appointments.
                            </p>

                            <Link
                                className="btn btn-outline-primary"
                                to="/appointments/management"
                            >
                                Manage Appointments
                            </Link>
                        </div>
                    </div>
                </div>
            </div>
        </section>
    );
}

export default MechanicDashboard;