import { useEffect, useState } from "react";
import { Link, useNavigate, useParams } from "react-router";
import { changeServiceOrderStatus, getServiceOrderById } from "../../api/serviceOrderApi";
import ServiceOrderItemManager from "../../components/serviceOrders/ServiceOrderItemManager";
import { useAuth } from "../../context/AuthContext";
import { getApiErrorMessage } from "../../utils/httpErrors";
import {
  formatServiceOrderItemType,
  formatServiceOrderStatus,
  normalizeServiceOrderStatus,
} from "../../utils/serviceOrderValues";

function formatDate(date) {
  if (!date) {
    return "Not available";
  }

  return new Date(date).toLocaleString();
}

function formatAmount(amount) {
  return Number(amount || 0).toFixed(2);
}

function getStatusColor(status) {
  const colors = {
    Pending: "warning",
    Approved: "info",
    InProgress: "primary",
    Completed: "success",
    Cancelled: "danger",
  };

  return colors[normalizeServiceOrderStatus(status)] || "secondary";
}

function ServiceOrderDetails() {
  const { id } = useParams();
  const navigate = useNavigate();
  const { user } = useAuth();

  const [serviceOrder, setServiceOrder] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");
  const [actionError, setActionError] = useState("");

  useEffect(() => {
    async function loadServiceOrder() {
      try {
        setLoading(true);
        setError("");

        const response = await getServiceOrderById(id);
        setServiceOrder(response.data);
      } catch (requestError) {
        const backendMessage = requestError.response?.data;

        setError(
          typeof backendMessage === "string"
            ? backendMessage
            : "Could not load the service order."
        );
      } finally {
        setLoading(false);
      }
    }

    loadServiceOrder();
  }, [id]);

  if (loading) {
    return (
      <div className="text-center py-5">
        <div className="spinner-border text-primary" role="status">
          <span className="visually-hidden">Loading...</span>
        </div>
        <p className="mt-2">Loading service order...</p>
      </div>
    );
  }

  if (error) {
    return (
      <div className="alert alert-danger" role="alert">
        {error}
      </div>
    );
  }

  if (!serviceOrder) {
    return (
      <div className="alert alert-info" role="alert">
        Service-order information is not available.
      </div>
    );
  }

  const orderItems = serviceOrder.serviceOrderItems || [];
  const vehicle = serviceOrder.vehicle;
  const canManage = ["Admin", "Mechanic"].includes(user?.role);
  const normalizedStatus = normalizeServiceOrderStatus(serviceOrder.status);
  const hasLegacyInProgressStatus = serviceOrder.status === "In Progress";
  const validTransitions = {
    Pending: ["Approved", "Cancelled"],
    Approved: ["InProgress", "Cancelled"],
    InProgress: ["Completed", "Cancelled"],
  };
  const availableTransitions = hasLegacyInProgressStatus
    ? []
    : validTransitions[normalizedStatus] || [];

  async function updateStatus(newStatus) {
    try {
      setActionError("");
      await changeServiceOrderStatus(id, newStatus);
      const response = await getServiceOrderById(id);
      setServiceOrder(response.data);
    } catch (requestError) {
      setActionError(getApiErrorMessage(requestError, "The status transition is not allowed."));
    }
  }

  return (
    <section>
      <div className="d-flex justify-content-between align-items-center mb-4">
        <div>
          <h2 className="mb-1">
            Service Order #{serviceOrder.serviceOrderId}
          </h2>
          <p className="text-secondary mb-0">
            Created {formatDate(serviceOrder.createdAt)}
          </p>
        </div>

        <div className="d-flex gap-2 align-items-center"><span className={`badge text-bg-${getStatusColor(serviceOrder.status)}`}>{formatServiceOrderStatus(serviceOrder.status)}</span>{canManage && <Link className="btn btn-outline-primary btn-sm" to={`/service-orders/${id}/edit`}>Edit order</Link>}</div>
      </div>

      {actionError && <div className="alert alert-danger">{actionError}</div>}
      {hasLegacyInProgressStatus && canManage && <div className="alert alert-warning">This order uses an older status value. Its status must be corrected in the backend before it can move to Completed or Cancelled.</div>}
      {canManage && <div className="card card-body shadow-sm mb-4"><div className="d-flex flex-wrap align-items-center gap-2"><strong>Status transition:</strong>{availableTransitions.length > 0 ? availableTransitions.map((status) => <button className="btn btn-outline-secondary btn-sm" type="button" key={status} onClick={() => updateStatus(status)}>{formatServiceOrderStatus(status)}</button>) : <span className="text-secondary">No further transitions are available.</span>}</div></div>}

      <div className="row g-4 mb-4">
        <div className="col-lg-7">
          <div className="card h-100 shadow-sm">
            <div className="card-header bg-white">
              <h3 className="h5 mb-0">Order Information</h3>
            </div>

            <div className="card-body">
              <div className="row g-3">
                <div className="col-md-6">
                  <p className="text-secondary mb-1">Order Date</p>
                  <p className="mb-0">
                    {formatDate(serviceOrder.orderDate)}
                  </p>
                </div>

                <div className="col-md-6">
                  <p className="text-secondary mb-1">Completion Date</p>
                  <p className="mb-0">
                    {formatDate(serviceOrder.completionDate)}
                  </p>
                </div>

                <div className="col-md-4">
                  <p className="text-secondary mb-1">Customer ID</p>
                  <p className="mb-0">{serviceOrder.customerProfileId}</p>
                </div>

                <div className="col-md-4">
                  <p className="text-secondary mb-1">Mechanic ID</p>
                  <p className="mb-0">
                    {serviceOrder.mechanicProfileId || "Not assigned"}
                  </p>
                </div>

                <div className="col-md-4">
                  <p className="text-secondary mb-1">Branch ID</p>
                  <p className="mb-0">{serviceOrder.branchId}</p>
                </div>

                <div className="col-12">
                  <p className="text-secondary mb-1">Customer Complaint</p>
                  <p className="mb-0">
                    {serviceOrder.customerComplaint || "No complaint provided"}
                  </p>
                </div>

                <div className="col-12">
                  <p className="text-secondary mb-1">Diagnosis</p>
                  <p className="mb-0">
                    {serviceOrder.diagnosis || "No diagnosis provided"}
                  </p>
                </div>
              </div>
            </div>
          </div>
        </div>

        <div className="col-lg-5">
          <div className="card h-100 shadow-sm">
            <div className="card-header bg-white">
              <h3 className="h5 mb-0">Vehicle</h3>
            </div>

            <div className="card-body">
              {vehicle ? (
                <div className="row g-3">
                  <div className="col-12">
                    <p className="text-secondary mb-1">Vehicle</p>
                    <p className="mb-0">
                      {vehicle.year} {vehicle.make} {vehicle.model}
                    </p>
                  </div>

                  <div className="col-sm-6">
                    <p className="text-secondary mb-1">Plate Number</p>
                    <p className="mb-0">{vehicle.plateNumber}</p>
                  </div>

                  <div className="col-sm-6">
                    <p className="text-secondary mb-1">Color</p>
                    <p className="mb-0">{vehicle.color || "Not available"}</p>
                  </div>

                  <div className="col-sm-6">
                    <p className="text-secondary mb-1">Mileage</p>
                    <p className="mb-0">
                      {vehicle.mileage ?? "Not available"}
                    </p>
                  </div>

                  <div className="col-sm-6">
                    <p className="text-secondary mb-1">VIN</p>
                    <p className="mb-0">{vehicle.vin || "Not available"}</p>
                  </div>
                </div>
              ) : (
                <p className="text-secondary mb-0">
                  Vehicle information is not available.
                </p>
              )}
            </div>
          </div>
        </div>
      </div>

      <div className="card shadow-sm mb-4">
        <div className="card-header bg-white d-flex justify-content-between">
          <h3 className="h5 mb-0">Order Items</h3>
          <span className="badge text-bg-secondary">
            {orderItems.length}
          </span>
        </div>

        <div className="card-body p-0">
          {orderItems.length === 0 ? (
            <p className="text-secondary p-3 mb-0">
              No items have been added to this service order.
            </p>
          ) : (
            <div className="table-responsive">
              <table className="table table-striped align-middle mb-0">
                <thead>
                  <tr>
                    <th>Type</th>
                    <th>Description</th>
                    <th>Quantity</th>
                    <th>Unit Price</th>
                    <th>Labor Hours</th>
                    <th>Subtotal</th>
                  </tr>
                </thead>
                <tbody>
                  {orderItems.map((item) => (
                    <tr key={item.serviceOrderItemId}>
                      <td>{formatServiceOrderItemType(item.itemType)}</td>
                      <td>{item.description || "Not available"}</td>
                      <td>{item.quantity}</td>
                      <td>{formatAmount(item.unitPrice)}</td>
                      <td>{item.laborHours ?? "-"}</td>
                      <td>{formatAmount(item.subtotal)}</td>
                    </tr>
                  ))}
                </tbody>
                <tfoot>
                  <tr className="table-light">
                    <th colSpan="5">Order Total</th>
                    <th>{formatAmount(serviceOrder.totalAmount)}</th>
                  </tr>
                </tfoot>
              </table>
            </div>
          )}
        </div>
      </div>

      {canManage && <ServiceOrderItemManager serviceOrderId={Number(id)} />}

      <button
        type="button"
        className="btn btn-outline-secondary"
        onClick={() => navigate(-1)}
      >
        Back
      </button>
    </section>
  );
}

export default ServiceOrderDetails;
