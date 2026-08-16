import { Link } from "react-router";

function Home() {
    return (
        <>
            <section className="public-hero">
                <div className="container">
                    <div className="row hero-inner g-5">
                        <div className="col-lg-7">
                            <p className="hero-eyebrow mb-3">Professional automotive care</p>
                            <h1 className="hero-title mb-4">Keep every journey <span>running smoothly.</span></h1>
                            <p className="hero-lead mb-4">Reliable maintenance, skilled mechanics, and transparent service—everything your vehicle needs under one roof.</p>
                            <div className="hero-actions d-flex flex-wrap gap-3">
                                <Link to="/services" className="btn btn-accent btn-lg"><i className="bi bi-tools me-2" aria-hidden="true" />Explore Services</Link>
                                <Link to="/branches" className="btn btn-outline-light btn-lg"><i className="bi bi-geo-alt me-2" aria-hidden="true" />Find a Branch</Link>
                            </div>
                            <div className="hero-trust">
                                <span><i className="bi bi-patch-check-fill" />Certified mechanics</span>
                                <span><i className="bi bi-shield-check" />Trusted service</span>
                                <span><i className="bi bi-receipt" />Clear pricing</span>
                            </div>
                        </div>
                        <div className="col-lg-5 d-flex align-items-center">
                            <img className="hero-brand-image" src="/vehicle-service-center-logo.png" alt="Vehicle Service Center automotive shield logo" />
                        </div>
                    </div>
                </div>
            </section>

            <section className="public-section">
                <div className="container">
                    <div className="text-center mb-5">
                        <p className="section-eyebrow mb-2">Why choose us</p>
                        <h2 className="mb-2">Service built around your confidence</h2>
                        <p className="text-muted mb-0">Modern care for every make, model, and mile.</p>
                    </div>
                    <div className="row g-4">
                        {[
                            ["person-gear", "Expert Mechanics", "Certified technicians with the experience to diagnose and repair with confidence."],
                            ["geo-alt", "Convenient Branches", "Professional service locations designed to keep your day moving."],
                            ["cash-coin", "Fair Pricing", "Transparent quotes and straightforward recommendations with no surprises."],
                        ].map(([icon, title, text]) => (
                            <div className="col-md-4" key={title}>
                                <div className="card feature-card h-100">
                                    <div className="feature-icon"><i className={`bi bi-${icon}`} aria-hidden="true" /></div>
                                    <h3 className="h5">{title}</h3>
                                    <p className="text-muted mb-0">{text}</p>
                                </div>
                            </div>
                        ))}
                    </div>
                </div>
            </section>
        </>
    );
}

export default Home;
