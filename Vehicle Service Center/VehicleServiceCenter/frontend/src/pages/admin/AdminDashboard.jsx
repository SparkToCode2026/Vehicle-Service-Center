import { useEffect, useState } from "react";
import { Link } from "react-router";
import { getServiceOrderSummary } from "../../api/serviceOrderApi";

function formatAmount(amount) {
  return Number(amount || 0).toFixed(2);
}

function formatStatus(status) {
  return status.replace(/([a-z])([A-Z])/g, "$1 $2");
}

function getStatusColor(status) {
  const colors = {
    Pending: "warning",
    Approved: "info",
    InProgress: "primary",
    Completed: "success",
    Cancelled: "danger",
  };

  return colors[status] || "secondary";
}

function AdminDashboard() {
  const [summary, setSummary] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  useEffect(() => {
    async function loadDashboard() {
      try {
        const response = await getServiceOrderSummary();
        setSummary(response.data);
      } catch {
        setError("Could not load the dashboard information.");
      } finally {
        setLoading(false);
      }
    }

    loadDashboard();
  }, []);

  const totalOrders = summary.reduce(
    (total, item) => total + item.count,
    0
  );

  const totalRevenue = summary.reduce(
    (total, item) => total + item.totalRevenue,
    0
  );

  return (
    <section>
      <div className="page-header d-flex justify-content-between align-items-center gap-3">
        <div>
          <h2 className="mb-1">Admin Dashboard</h2>
          <p className="text-secondary mb-0">
            Overview of vehicle service orders
          </p>
        </div>

        <Link className="btn btn-primary btn-icon-label" to="/service-orders">
          <i className="bi bi-clipboard-check" aria-hidden="true" />
          View Service Orders
        </Link>
      </div>

      {loading && (
        <div className="text-center py-5">
          <div className="spinner-border text-primary" role="status">
            <span className="visually-hidden">Loading...</span>
          </div>
          <p className="mt-2">Loading dashboard...</p>
        </div>
      )}

      {error && <div className="alert alert-danger">{error}</div>}

      {!loading && !error && (
        <>
          <div className="row g-3 mb-4">
            <div className="col-md-6">
              <div className="stat-card d-flex align-items-center gap-3">
                <div className="stat-icon"><i className="bi bi-clipboard-data" aria-hidden="true" /></div>
                <div><p className="stat-label mb-1">Total Service Orders</p><h3 className="stat-value mb-0">{totalOrders}</h3></div>
              </div>
            </div>

            <div className="col-md-6">
              <div className="stat-card d-flex align-items-center gap-3">
                <div className="stat-icon stat-icon-accent"><i className="bi bi-cash-stack" aria-hidden="true" /></div>
                <div><p className="stat-label mb-1">Total Revenue</p><h3 className="stat-value mb-0">{formatAmount(totalRevenue)}</h3></div>
              </div>
            </div>
          </div>

          <h3 className="h5 mb-3">Orders by Status</h3>

          {summary.length === 0 ? (
            <div className="alert alert-info">
              No service-order information is available yet.
            </div>
          ) : (
            <div className="row g-3">
              {summary.map((item) => (
                <div className="col-sm-6 col-xl-4" key={item.status}>
                  <div className="card dashboard-card h-100">
                    <div className="card-body">
                      <span
                        className={`badge text-bg-${getStatusColor(item.status)}`}
                      >
                        {formatStatus(item.status)}
                      </span>

                      <h4 className="mt-3 mb-1">{item.count} orders</h4>
                      <p className="text-secondary mb-0">
                        Revenue: {formatAmount(item.totalRevenue)}
                      </p>
                    </div>
                  </div>
                </div>
              ))}
            </div>
          )}
        </>
      )}
    </section>
  );
}

export default AdminDashboard;
