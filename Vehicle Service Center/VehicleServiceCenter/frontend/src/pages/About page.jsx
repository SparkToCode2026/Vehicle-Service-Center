const VALUES = [
    {
        title: "Integrity",
        text: "We diagnose honestly and only recommend work your vehicle actually needs.",
    },
    {
        title: "Quality",
        text: "Genuine parts and trained mechanics, every time, no shortcuts.",
    },
    {
        title: "Customer First",
        text: "Clear communication and fair pricing, so you always know what to expect.",
    },
];

function About() {
    return (
        <>
            {/* Page header */}
            <section className="bg-dark text-white py-5">
                <div className="container text-center py-4">
                    <h1 className="fw-bold mb-3">About Us</h1>
                    <p className="lead text-light-emphasis mx-auto" style={{ maxWidth: "640px" }}>
                        A trusted name in vehicle maintenance and repair, built on years of
                        experience and a commitment to quality.
                    </p>
                </div>
            </section>

            {/* Story */}
            <section className="py-5">
                <div className="container">
                    <div className="row align-items-center g-5">
                        <div className="col-lg-6">
                            <h2 className="fw-bold mb-3">Our Story</h2>
                            <p className="text-muted">
                                Vehicle Service Center started with a simple goal: make quality
                                vehicle care accessible, honest, and stress-free. What began as
                                a single garage has grown into a network of branches, all
                                built on the same principle — treat every customer's vehicle
                                like our own.
                            </p>
                            <p className="text-muted mb-0">
                                Today, our certified mechanics handle everything from routine
                                oil changes to complex engine diagnostics, backed by
                                transparent pricing and real communication at every step.
                            </p>
                        </div>
                        <div className="col-lg-6">
                            <div className="row g-3 text-center">
                                <div className="col-6">
                                    <div className="border rounded-3 p-4 h-100">
                                        <div className="fs-2 fw-bold text-primary">10+</div>
                                        <div className="text-muted small">Years of Service</div>
                                    </div>
                                </div>
                                <div className="col-6">
                                    <div className="border rounded-3 p-4 h-100">
                                        <div className="fs-2 fw-bold text-primary">2</div>
                                        <div className="text-muted small">Branches</div>
                                    </div>
                                </div>
                                <div className="col-6">
                                    <div className="border rounded-3 p-4 h-100">
                                        <div className="fs-2 fw-bold text-primary">1000+</div>
                                        <div className="text-muted small">Vehicles Served</div>
                                    </div>
                                </div>
                                <div className="col-6">
                                    <div className="border rounded-3 p-4 h-100">
                                        <div className="fs-2 fw-bold text-primary">4.8★</div>
                                        <div className="text-muted small">Customer Rating</div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </section>

            {/* Values */}
            <section className="bg-light py-5">
                <div className="container">
                    <h2 className="text-center fw-bold mb-5">What We Stand For</h2>
                    <div className="row g-4">
                        {VALUES.map((value) => (
                            <div className="col-md-4" key={value.title}>
                                <div className="card h-100 border-0 shadow-sm p-4">
                                    <h5 className="fw-semibold mb-2">{value.title}</h5>
                                    <p className="text-muted small mb-0">{value.text}</p>
                                </div>
                            </div>
                        ))}
                    </div>
                </div>
            </section>
        </>
    );
}

export default About;