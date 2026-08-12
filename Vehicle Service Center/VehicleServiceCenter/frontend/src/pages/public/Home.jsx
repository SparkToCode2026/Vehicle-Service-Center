import { Link } from "react-router";

function Home() {
    return (
        <div className="container py-5">
            <div className="text-center py-5">
                <h1 className="fw-bold mb-3">Vehicle Care You Can Trust</h1>
                <p className="lead text-muted mb-4">
                    Reliable maintenance and repair services from certified mechanics.
                </p>
                <div className="d-flex gap-3 justify-content-center">
                    <Link to="/services" className="btn btn-primary">
                        Our Services
                    </Link>
                    <Link to="/branches" className="btn btn-outline-primary">
                        Find a Branch
                    </Link>
                </div>
            </div>

            <div className="row g-4 mt-4">
                <div className="col-md-4">
                    <div className="card h-100 text-center p-3">
                        <h5 className="fw-semibold">Expert Mechanics</h5>
                        <p className="text-muted small mb-0">
                            Certified technicians with years of experience.
                        </p>
                    </div>
                </div>
                <div className="col-md-4">
                    <div className="card h-100 text-center p-3">
                        <h5 className="fw-semibold">Multiple Branches</h5>
                        <p className="text-muted small mb-0">
                            Convenient locations close to you.
                        </p>
                    </div>
                </div>
                <div className="col-md-4">
                    <div className="card h-100 text-center p-3">
                        <h5 className="fw-semibold">Fair Pricing</h5>
                        <p className="text-muted small mb-0">
                            Transparent quotes, no hidden fees.
                        </p>
                    </div>
                </div>
            </div>
        </div>
    );
}

export default Home;
