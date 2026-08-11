import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router";
import { getServiceOrderById } from "../api/serviceOrderApi";

function formatDate(date) {
  if (!date) {
    return "Not available";
  }

  return new Date(date).toLocaleString();
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

function ServiceOrderDetails() {
  const { id } = useParams();
  const navigate = useNavigate();

  const [serviceOrder, setServiceOrder] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

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

        <span
          className={`badge text-bg-${getStatusColor(serviceOrder.status)}`}
        >
          {formatStatus(serviceOrder.status)}
        </span>
      </div>

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
                      <td>{item.itemType}</td>
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
