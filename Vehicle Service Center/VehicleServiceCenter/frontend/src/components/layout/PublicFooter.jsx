import { Link } from "react-router";
import BrandLogo from "./BrandLogo";

function PublicFooter() {
  return (
    <footer className="public-footer text-light mt-auto">
      <div className="container py-5">
        <div className="row g-4 align-items-start">
          <div className="col-md-6">
            <BrandLogo light />
            <p className="text-white-50 mb-0">
              Reliable vehicle maintenance and repair services you can trust.
            </p>
          </div>

          <div className="col-6 col-md-3">
            <h2 className="h6 text-light mb-2">Explore</h2>
            <nav className="d-flex flex-column gap-1" aria-label="Footer navigation">
              <Link className="link-light link-offset-2" to="/about">About</Link>
              <Link className="link-light link-offset-2" to="/services">Services</Link>
              <Link className="link-light link-offset-2" to="/branches">Branches</Link>
            </nav>
          </div>

          <div className="col-6 col-md-3">
            <h2 className="h6 text-light mb-2">Account</h2>
            <nav className="d-flex flex-column gap-1" aria-label="Account navigation">
              <Link className="link-light link-offset-2" to="/login">Sign In</Link>
              <Link className="link-light link-offset-2" to="/register">Create Account</Link>
            </nav>
          </div>
        </div>

        <div className="footer-bottom border-top mt-4 pt-3 d-flex flex-wrap gap-2 justify-content-between text-white-50 small">
          &copy; {new Date().getFullYear()} Vehicle Service Center. All rights reserved.
          <span>Designed for safer journeys.</span>
        </div>
      </div>
    </footer>
  );
}

export default PublicFooter;
