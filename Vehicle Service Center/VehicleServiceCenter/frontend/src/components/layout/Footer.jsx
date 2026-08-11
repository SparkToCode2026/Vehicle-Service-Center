import { Link } from "react-router";

function Footer() {
    const year = new Date().getFullYear();

    return (
        <footer className="bg-dark text-light pt-5 pb-4 mt-5">
            <div className="container">
                <div className="row gy-4">
                    <div className="col-lg-4 col-md-6">
                        <h5 className="fw-bold mb-3">Vehicle Service Center</h5>
                        <p className="text-secondary mb-0">
                            Reliable vehicle maintenance and repair services, trusted by
                            customers across our branches.
                        </p>
                    </div>

                    <div className="col-lg-2 col-md-6">
                        <h6 className="fw-semibold mb-3">Quick Links</h6>
                        <ul className="list-unstyled d-flex flex-column gap-2">
                            <li>
                                <Link to="/" className="text-secondary text-decoration-none">
                                    Home
                                </Link>
                            </li>
                            <li>
                                <Link to="/about" className="text-secondary text-decoration-none">
                                    About
                                </Link>
                            </li>
                            <li>
                                <Link to="/services" className="text-secondary text-decoration-none">
                                    Services
                                </Link>
                            </li>
                            <li>
                                <Link to="/branches" className="text-secondary text-decoration-none">
                                    Branches
                                </Link>
                            </li>
                        </ul>
                    </div>

                    <div className="col-lg-3 col-md-6">
                        <h6 className="fw-semibold mb-3">Contact</h6>
                        <ul className="list-unstyled text-secondary d-flex flex-column gap-2">
                            <li>Muscat, Oman</li>
                            <li>+968 9999 5555</li>
                            <li>main@vehicleservice.com</li>
                        </ul>
                    </div>

                    <div className="col-lg-3 col-md-6">
                        <h6 className="fw-semibold mb-3">Hours</h6>
                        <ul className="list-unstyled text-secondary d-flex flex-column gap-2">
                            <li>Sat &ndash; Thu: 8:00 AM &ndash; 6:00 PM</li>
                            <li>Friday: Closed</li>
                        </ul>
                    </div>
                </div>

                <hr className="border-secondary mt-4" />

                <p className="text-secondary text-center mb-0 small">
                    &copy; {year} Vehicle Service Center. All rights reserved.
                </p>
            </div>
        </footer>
    );
}

export default Footer;