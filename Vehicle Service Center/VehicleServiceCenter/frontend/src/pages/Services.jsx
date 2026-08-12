import { useEffect, useState } from "react";
import api from "../api/api";
import LoadingSpinner from "../components/shared/LoadingSpinner";
import EmptyDataMessage from "../components/shared/EmptyDataMessage";
import StatusBadge from "../components/shared/StatusBadge";

function Services() {
    const [services, setServices] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(false);

    useEffect(() => {
        api
            .get("/ServiceType")
            .then((res) => setServices(res.data))
            .catch(() => setError(true))
            .finally(() => setLoading(false));
    }, []);

    if (loading) return <LoadingSpinner message="Loading services..." fullPage />;

    return (
        <div className="container py-5">
            <div className="text-center mb-4">
                <h1 className="fw-bold">Our Services</h1>
                <p className="text-muted">Professional vehicle care you can rely on.</p>
            </div>

            {error && (
                <div className="alert alert-danger text-center">
                    Couldn't load services right now.
                </div>
            )}

            {!error && services.length === 0 && (
                <EmptyDataMessage title="No services available" />
            )}

            {!error && services.length > 0 && (
                <div className="row g-4">
                    {services.map((s) => (
                        <div className="col-md-4" key={s.serviceTypeId}>
                            <div className="card h-100 p-3">
                                <div className="d-flex justify-content-between align-items-start">
                                    <h5 className="fw-semibold">{s.name}</h5>
                                    <StatusBadge status={s.isActive ? "Active" : "Inactive"} />
                                </div>
                                <p className="text-muted small">{s.description}</p>
                                <p className="fw-bold text-primary mb-0">${s.basePrice}</p>
                            </div>
                        </div>
                    ))}
                </div>
            )}
        </div>
    );
}

export default Services;