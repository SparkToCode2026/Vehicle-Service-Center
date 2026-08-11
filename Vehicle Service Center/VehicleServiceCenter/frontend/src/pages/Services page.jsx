import { useEffect, useState } from "react";
import api from "../api/api";
import LoadingSpinner from "../components/common/LoadingSpinner";
import EmptyDataMessage from "../components/common/EmptyDataMessage";
import StatusBadge from "../components/common/StatusBadge";

function Services() {
    const [services, setServices] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    useEffect(() => {
        let cancelled = false;

        api
            .get("/api/ServiceType")
            .then((res) => {
                if (!cancelled) setServices(res.data);
            })
            .catch((err) => {
                if (!cancelled) setError(err);
            })
            .finally(() => {
                if (!cancelled) setLoading(false);
            });

        // Cleanup flag: if the component unmounts before the request finishes
        // (e.g. user navigates away fast), we skip the state update instead of
        // calling setState on an unmounted component.
        return () => {
            cancelled = true;
        };
    }, []);

    return (
        <div className="container py-5">
            <div className="text-center mb-5">
                <h1 className="fw-bold">Our Services</h1>
                <p className="text-muted">
                    Professional vehicle care, from routine maintenance to full diagnostics.
                </p>
            </div>

            {loading && <LoadingSpinner message="Loading services..." fullPage />}

            {!loading && error && (
                <div className="alert alert-danger text-center">
                    Couldn't load services right now. Please try again later.
                </div>
            )}

            {!loading && !error && services.length === 0 && (
                <EmptyDataMessage
                    title="No services available"
                    message="Check back soon for our full list of services."
                />
            )}

            {!loading && !error && services.length > 0 && (
                <div className="row g-4">
                    {services.map((service) => (
                        <div className="col-md-6 col-lg-4" key={service.serviceTypeId}>
                            <div className="card h-100 border-0 shadow-sm">
                                <div className="card-body d-flex flex-column">
                                    <div className="d-flex justify-content-between align-items-start mb-2">
                                        <h5 className="card-title fw-semibold mb-0">{service.name}</h5>
                                        <StatusBadge status={service.isActive ? "Active" : "Inactive"} />
                                    </div>
                                    <p className="text-muted small flex-grow-1">{service.description}</p>
                                    <div className="d-flex justify-content-between align-items-center mt-3">
                                        <span className="fw-bold text-primary">${service.basePrice}</span>
                                        <span className="text-muted small">
                      ~{service.estimatedDurationMinutes} min
                    </span>
                                    </div>
                                </div>
                            </div>
                        </div>
                    ))}
                </div>
            )}
        </div>
    );
}

export default Services;