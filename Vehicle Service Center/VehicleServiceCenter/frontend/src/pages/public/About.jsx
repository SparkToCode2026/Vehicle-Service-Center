function About() {
    return (
        <div className="container public-section">
            <div className="text-center mb-5">
                <p className="section-eyebrow mb-2">Our story</p>
                <h1 className="fw-bold">About Us</h1>
                <p className="text-muted">Trusted vehicle care since day one.</p>
            </div>

            <div className="row justify-content-center">
                <div className="col-lg-8">
                    <div className="card p-4 p-md-5">
                    <div className="feature-icon"><i className="bi bi-shield-check" aria-hidden="true" /></div>
                    <p className="text-muted">
                        Vehicle Service Center provides honest, reliable maintenance and
                        repair services. Our certified mechanics handle everything from
                        routine oil changes to full diagnostics, backed by transparent
                        pricing and clear communication.
                    </p>
                    </div>
                    <p className="text-muted mb-0">
                        We believe in treating every customer's vehicle like our own —
                        no unnecessary work, no hidden costs, just quality service you
                        can count on.
                    </p>
                </div>
            </div>
        </div>
    );
}

export default About;
