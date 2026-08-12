import { useEffect, useState } from "react";
import { Link } from "react-router";

import {
  deleteServiceOrder,
  filterServiceOrders,
  getAllServiceOrders,
  getServiceOrdersByMechanic,
} from "../api/serviceOrderApi";
import { getMechanicProfileByUserId } from "../api/mechanicProfileApi";

import { useAuth } from "../context/AuthContext";



function formatDate(date) {
  return new Date(date).toLocaleDateString();
}

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

function ServiceOrderList() {
  const { user } = useAuth();

  const [serviceOrders, setServiceOrders] = useState([]);
  const [status, setStatus] = useState("");
  const [fromDate, setFromDate] = useState("");
  const [toDate, setToDate] = useState("");
  const [loading, setLoading] = useState(true);
  const [deletingId, setDeletingId] = useState(null);
  const [error, setError] = useState("");

  async function loadServiceOrders(filters) {
    try {
      setLoading(true);
      setError("");

      

      const response = filters
          ? await filterServiceOrders(filters)
          : await getAllServiceOrders();

      setServiceOrders(response.data);
      
    } catch {
      setError("Could not load the service orders.");
    } finally {
      setLoading(false);
    }
  }

  useEffect(() => {
    async function loadData() {
      try {
        setLoading(true);
        setError("");

        if (user?.role === "Mechanic") {
          const response = await getMechanicProfileByUserId(user.userId);

          const profileId = response.data.mechanicProfileId;

          

          const ordersResponse =
              await getServiceOrdersByMechanic(profileId);

          setServiceOrders(ordersResponse.data);
        } else {
          await loadServiceOrders();
        }
      } catch (error) {
        console.error(error);
        setError("Could not load the service orders.");
      } finally {
        setLoading(false);
      }
    }

    if (user) {
      loadData();
    }
  }, [user]);
  
  function handleFilter(event) {
    event.preventDefault();

    loadServiceOrders({
      status: status || undefined,
      from: fromDate ? `${fromDate}T00:00:00` : undefined,
      to: toDate ? `${toDate}T23:59:59` : undefined,
    });
  }

  function clearFilters() {
    setStatus("");
    setFromDate("");
    setToDate("");
    loadServiceOrders();
  }

  async function handleDelete(serviceOrderId) {
    const confirmed = window.confirm(
      `Delete service order #${serviceOrderId}?`
    );

    if (!confirmed) {
      return;
    }

    try {
      setDeletingId(serviceOrderId);
      setError("");

      await deleteServiceOrder(serviceOrderId);

      setServiceOrders((currentOrders) =>
        currentOrders.filter(
          (order) => order.serviceOrderId !== serviceOrderId
        )
      );
    } catch {
      setError("Could not delete the service order.");
    } finally {
      setDeletingId(null);
    }
  }

  return (
    <section>
      <div className="mb-4">
        <h2 className="mb-1">Service Orders</h2>
        <p className="text-secondary mb-0">
          View and manage vehicle service orders
        </p>
      </div>

      <div className="card shadow-sm mb-4">
        <div className="card-body">
          <form className="row g-3 align-items-end" onSubmit={handleFilter}>
            <div className="col-md-4">
              <label className="form-label" htmlFor="status">
                Status
              </label>
              <select
                id="status"
                className="form-select"
                value={status}
                onChange={(event) => setStatus(event.target.value)}
              >
                <option value="">All statuses</option>
                <option value="Pending">Pending</option>
                <option value="Approved">Approved</option>
                <option value="InProgress">In Progress</option>
                <option value="Completed">Completed</option>
                <option value="Cancelled">Cancelled</option>
              </select>
            </div>

            <div className="col-md-3">
              <label className="form-label" htmlFor="fromDate">
                From date
              </label>
              <input
                id="fromDate"
                className="form-control"
                type="date"
                value={fromDate}
                onChange={(event) => setFromDate(event.target.value)}
              />
            </div>

            <div className="col-md-3">
              <label className="form-label" htmlFor="toDate">
                To date
              </label>
              <input
                id="toDate"
                className="form-control"
                type="date"
                value={toDate}
                onChange={(event) => setToDate(event.target.value)}
              />
            </div>

            <div className="col-md-2 d-flex gap-2">
              <button className="btn btn-primary" type="submit">
                Filter
              </button>
              <button
                className="btn btn-outline-secondary"
                type="button"
                onClick={clearFilters}
              >
                Clear
              </button>
            </div>
          </form>
        </div>
      </div>

      {error && (
        <div className="alert alert-danger" role="alert">
          {error}
        </div>
      )}

      {loading ? (
        <div className="text-center py-5">
          <div className="spinner-border text-primary" role="status">
            <span className="visually-hidden">Loading...</span>
          </div>
          <p className="mt-2">Loading service orders...</p>
        </div>
      ) : (
        <div className="card shadow-sm">
          <div className="card-body p-0">
            {serviceOrders.length === 0 ? (
              <p className="text-secondary text-center p-4 mb-0">
                No service orders were found.
              </p>
            ) : (
              <div className="table-responsive">
                <table className="table table-hover align-middle mb-0">
                  <thead className="table-light">
                    <tr>
                      <th>Order</th>
                      <th>Vehicle</th>
                      <th>Order Date</th>
                      <th>Status</th>
                      <th>Items</th>
                      <th>Total</th>
                      <th>Actions</th>
                    </tr>
                  </thead>
                  <tbody>
                    {serviceOrders.map((order) => (
                      <tr key={order.serviceOrderId}>
                        <td>#{order.serviceOrderId}</td>
                        <td>
                          {order.vehicle
                            ? `${order.vehicle.make} ${order.vehicle.model}`
                            : `Vehicle #${order.vehicleId}`}
                        </td>
                        <td>{formatDate(order.orderDate)}</td>
                        <td>
                          <span
                            className={`badge text-bg-${getStatusColor(order.status)}`}
                          >
                            {formatStatus(order.status)}
                          </span>
                        </td>
                        <td>{order.serviceOrderItems?.length || 0}</td>
                        <td>{formatAmount(order.totalAmount)}</td>
                        <td>
                          <div className="d-flex gap-2">
                            <Link
                              className="btn btn-outline-primary btn-sm"
                              to={`/service-orders/${order.serviceOrderId}`}
                            >
                              Details
                            </Link>

                            {user?.role === "Admin" && (
                              <button
                                className="btn btn-outline-danger btn-sm"
                                type="button"
                                disabled={deletingId === order.serviceOrderId}
                                onClick={() =>
                                  handleDelete(order.serviceOrderId)
                                }
                              >
                                {deletingId === order.serviceOrderId
                                  ? "Deleting..."
                                  : "Delete"}
                              </button>
                            )}
                          </div>
                        </td>
                      </tr>
                    ))}
                  </tbody>
                </table>
              </div>
            )}
          </div>
        </div>
      )}
    </section>
  );
}

export default ServiceOrderList;
