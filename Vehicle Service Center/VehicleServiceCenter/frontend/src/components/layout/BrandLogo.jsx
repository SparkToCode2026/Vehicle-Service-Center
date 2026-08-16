import { Link } from "react-router";

function BrandLogo({ to = "/", compact = false, light = false }) {
  return (
    <Link
      className={`brand-logo ${compact ? "brand-logo-compact" : ""} ${light ? "brand-logo-light" : ""}`}
      to={to}
      aria-label="Vehicle Service Center home"
    >
      <span className="brand-mark" aria-hidden="true">
        <i className="bi bi-shield-fill" />
        <i className="bi bi-car-front-fill" />
      </span>
      <span className="brand-copy">
        <span className="brand-name">Vehicle</span>
        <span className="brand-subtitle">Service Center</span>
      </span>
    </Link>
  );
}

export default BrandLogo;
