import { useEffect, useState } from "react";
import api from "../../api/api";
import LoadingSpinner from "../../components/shared/LoadingSpinner";
import EmptyDataMessage from "../../components/shared/EmptyDataMessage";
import StatusBadge from "../../components/shared/StatusBadge";

function Branches() {
    const [branches, setBranches] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(false);

    useEffect(() => {
        api
            .get("/Branch/GetAll")
            .then((res) => setBranches(res.data))
            .catch(() => setError(true))
            .finally(() => setLoading(false));
    }, []);

    if (loading) return <LoadingSpinner message="Loading branches..." fullPage />;

    return (
        <div className="container public-section">
            <div className="text-center mb-5">
                <p className="section-eyebrow mb-2">Visit us</p>
                <h1 className="fw-bold">Our Branches</h1>
                <p className="text-muted">Find a location near you.</p>
            </div>

            {error && (
                <div className="alert alert-danger text-center">
                    Couldn't load branches right now.
                </div>
            )}

            {!error && branches.length === 0 && (
                <EmptyDataMessage title="No branches available" />
            )}

            {!error && branches.length > 0 && (
                <div className="row g-4">
                    {branches.map((b) => (
                        <div className="col-md-6" key={b.branchId}>
                            <div className="card branch-card h-100 p-4">
                                <div className="d-flex justify-content-between align-items-start">
                                    <h5 className="fw-semibold">{b.branchName}</h5>
                                    <StatusBadge status={b.isActive ? "Active" : "Inactive"} />
                                </div>
                                <div className="branch-detail"><i className="bi bi-geo-alt" aria-hidden="true" /><span>{b.address}</span></div>
                                <div className="branch-detail"><i className="bi bi-telephone" aria-hidden="true" /><span>{b.phoneNumber}</span></div>
                                <div className="branch-detail mb-0"><i className="bi bi-clock" aria-hidden="true" /><span>{b.openingTime} &ndash; {b.closingTime}</span></div>
                            </div>
                        </div>
                    ))}
                </div>
            )}
        </div>
    );
}

export default Branches;
