function About() {
    return (
        <div className="container py-5">
            <div className="text-center mb-4">
                <h1 className="fw-bold">About Us</h1>
                <p className="text-muted">Trusted vehicle care since day one.</p>
            </div>

            <div className="row justify-content-center">
                <div className="col-md-8">
                    <p className="text-muted">
                        Vehicle Service Center provides honest, reliable maintenance and
                        repair services. Our certified mechanics handle everything from
                        routine oil changes to full diagnostics, backed by transparent
                        pricing and clear communication.
                    </p>
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