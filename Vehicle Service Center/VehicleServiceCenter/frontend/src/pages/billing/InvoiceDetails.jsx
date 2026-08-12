import { useEffect, useState } from "react";
import { Link, useNavigate, useParams } from "react-router";
import { getInvoiceById } from "../../api/invoiceApi";

function formatDate(date) {
  if (!date) {
    return "Not available";
  }

  return new Date(date).toLocaleDateString();
}

function formatAmount(amount) {
  return Number(amount || 0).toFixed(2);
}

function getStatusColor(status) {
  const colors = {
    Pending: "warning",
    Paid: "success",
    Overdue: "danger",
    Cancelled: "secondary",
  };

  return colors[status] || "primary";
}

function InvoiceDetails() {
  const { id } = useParams();
  const navigate = useNavigate();

  const [invoice, setInvoice] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  useEffect(() => {
    async function loadInvoice() {
      try {
        setLoading(true);
        setError("");

        const response = await getInvoiceById(id);
        setInvoice(response.data);
      } catch (requestError) {
        const backendMessage = requestError.response?.data;

        setError(
          typeof backendMessage === "string"
            ? backendMessage
            : "Could not load the invoice."
        );
      } finally {
        setLoading(false);
      }
    }

    loadInvoice();
  }, [id]);

  if (loading) {
    return (
      <div className="text-center py-5">
        <div className="spinner-border text-primary" role="status">
          <span className="visually-hidden">Loading...</span>
        </div>
        <p className="mt-2">Loading invoice...</p>
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

  if (!invoice) {
    return (
      <div className="alert alert-info" role="alert">
        Invoice information is not available.
      </div>
    );
  }

  return (
    <section>
      <div className="d-flex justify-content-between align-items-center mb-4">
        <div>
          <h2 className="mb-1">Invoice {invoice.invoiceNumber}</h2>
          <p className="text-secondary mb-0">
            Invoice ID: {invoice.invoiceId}
          </p>
        </div>

        <span className={`badge text-bg-${getStatusColor(invoice.status)}`}>
          {invoice.status}
        </span>
      </div>

      <div className="card shadow-sm mb-4">
        <div className="card-header bg-white">
          <h3 className="h5 mb-0">Invoice Information</h3>
        </div>

        <div className="card-body">
          <div className="row g-3">
            <div className="col-md-6">
              <p className="text-secondary mb-1">Service Order</p>
              <Link to={`/service-orders/${invoice.serviceOrderId}`}>
                #{invoice.serviceOrderId}
              </Link>
            </div>

            <div className="col-md-3">
              <p className="text-secondary mb-1">Issue Date</p>
              <p className="mb-0">{formatDate(invoice.issueDate)}</p>
            </div>

            <div className="col-md-3">
              <p className="text-secondary mb-1">Due Date</p>
              <p className="mb-0">{formatDate(invoice.dueDate)}</p>
            </div>
          </div>
        </div>
      </div>

      <div className="card shadow-sm mb-4">
        <div className="card-header bg-white">
          <h3 className="h5 mb-0">Payment Summary</h3>
        </div>

        <div className="card-body">
          <div className="table-responsive">
            <table className="table mb-0">
              <tbody>
                <tr>
                  <th scope="row">Subtotal</th>
                  <td className="text-end">
                    {formatAmount(invoice.subtotal)}
                  </td>
                </tr>
                <tr>
                  <th scope="row">Tax</th>
                  <td className="text-end">
                    {formatAmount(invoice.taxAmount)}
                  </td>
                </tr>
                <tr>
                  <th scope="row">Discount</th>
                  <td className="text-end">
                    - {formatAmount(invoice.discountAmount)}
                  </td>
                </tr>
                <tr className="table-light">
                  <th scope="row">Total</th>
                  <th className="text-end">
                    {formatAmount(invoice.totalAmount)}
                  </th>
                </tr>
              </tbody>
            </table>
          </div>
        </div>
      </div>

      {invoice.notes && (
        <div className="card shadow-sm mb-4">
          <div className="card-header bg-white">
            <h3 className="h5 mb-0">Notes</h3>
          </div>
          <div className="card-body">
            <p className="mb-0">{invoice.notes}</p>
          </div>
        </div>
      )}

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

export default InvoiceDetails;
