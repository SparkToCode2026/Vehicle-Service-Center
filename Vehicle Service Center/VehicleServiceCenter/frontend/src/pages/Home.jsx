import { Link } from "react-router";

const FEATURES = [
    {
        title: "Expert Mechanics",
        text: "Certified technicians with years of hands-on experience across all major vehicle brands.",
        icon: "🔧",
    },
    {
        title: "Multiple Branches",
        text: "Conveniently located service centers so you're never far from quality care.",
        icon: "📍",
    },
    {
        title: "Fast Turnaround",
        text: "Most routine services completed same-day, so you're back on the road quickly.",
        icon: "⚡",
    },
    {
        title: "Transparent Pricing",
        text: "No hidden fees. You approve the diagnosis and cost before any work begins.",
        icon: "💰",
    },
];

function Home() {
    return (
        <>
            {/* Hero */}
            <section
                className="text-white py-5"
                style={{
                    background: "linear-gradient(135deg, #0d1b2a 0%, #1b263b 60%, #0d6efd 140%)",
                }}
            >
                <div className="container py-5 text-center">
                    <h1 className="display-4 fw-bold mb-3">
                        Vehicle Care You Can Trust
                    </h1>
                    <p className="lead text-light-emphasis mb-4 mx-auto" style={{ maxWidth: "640px" }}>
                        From routine maintenance to complex repairs, our certified team keeps
                        your vehicle running safely and reliably.
                    </p>
                    <div className="d-flex gap-3 justify-content-center flex-wrap">
                        <Link to="/services" className="btn btn-primary btn-lg px-4">
                            Explore Services
                        </Link>
                        <Link to="/branches" className="btn btn-outline-light btn-lg px-4">
                            Find a Branch
                        </Link>
                    </div>
                </div>
            </section>

            {/* Features */}
            <section className="py-5">
                <div className="container">
                    <h2 className="text-center fw-bold mb-2">Why Choose Us</h2>
                    <p className="text-center text-muted mb-5">
                        Quality service, backed by a team that cares about your safety.
                    </p>

                    <div className="row g-4">
                        {FEATURES.map((feature) => (
                            <div className="col-md-6 col-lg-3" key={feature.title}>
                                <div className="card h-100 border-0 shadow-sm text-center p-3">
                                    <div className="card-body">
                                        <div className="fs-1 mb-3">{feature.icon}</div>
                                        <h5 className="card-title fw-semibold">{feature.title}</h5>
                                        <p className="card-text text-muted small">{feature.text}</p>
                                    </div>
                                </div>
                            </div>
                        ))}
                    </div>
                </div>
            </section>

            {/* CTA */}
            <section className="bg-light py-5">
                <div className="container text-center">
                    <h2 className="fw-bold mb-3">Ready to Book a Service?</h2>
                    <p className="text-muted mb-4">
                        Browse our services and find the nearest branch to get started.
                    </p>
                    <Link to="/services" className="btn btn-primary btn-lg px-5">
                        Get Started
                    </Link>
                </div>
            </section>
        </>
    );
}

export default Home;