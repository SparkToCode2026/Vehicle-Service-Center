import { Link } from "react-router";

function PublicFooter() {
  return (
    <footer className="bg-dark text-light mt-auto">
      <div className="container py-4">
        <div className="row g-4 align-items-start">
          <div className="col-md-6">
            <h2 className="h5 text-light mb-2">Vehicle Service Center</h2>
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

        <div className="border-top border-secondary mt-4 pt-3 text-center text-white-50 small">
          &copy; {new Date().getFullYear()} Vehicle Service Center. All rights reserved.
        </div>
      </div>
    </footer>
  );
}

export default PublicFooter;
