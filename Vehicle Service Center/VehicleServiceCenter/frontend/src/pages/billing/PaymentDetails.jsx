import { useEffect, useState } from "react";
import { Link, useNavigate, useParams } from "react-router";
import {
  getPaymentById,
  getTotalPaidForInvoice,
} from "../../api/paymentApi";

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
    Completed: "success",
    Successful: "success",
    Paid: "success",
    Failed: "danger",
    Refunded: "info",
    Cancelled: "secondary",
  };

  return colors[status] || "primary";
}

function PaymentDetails() {
  const { id } = useParams();
  const navigate = useNavigate();

  const [payment, setPayment] = useState(null);
  const [totalPaid, setTotalPaid] = useState(0);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  useEffect(() => {
    async function loadPayment() {
      try {
        setLoading(true);
        setError("");

        const paymentResponse = await getPaymentById(id);
        const paymentData = paymentResponse.data;

        setPayment(paymentData);

        const totalResponse = await getTotalPaidForInvoice(
          paymentData.invoiceId
        );

        setTotalPaid(totalResponse.data.totalPaid);
      } catch (requestError) {
        const backendMessage = requestError.response?.data;

        setError(
          typeof backendMessage === "string"
            ? backendMessage
            : "Could not load the payment."
        );
      } finally {
        setLoading(false);
      }
    }

    loadPayment();
  }, [id]);

  if (loading) {
    return (
      <div className="text-center py-5">
        <div className="spinner-border text-primary" role="status">
          <span className="visually-hidden">Loading...</span>
        </div>
        <p className="mt-2">Loading payment...</p>
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

  if (!payment) {
    return (
      <div className="alert alert-info" role="alert">
        Payment information is not available.
      </div>
    );
  }

  return (
    <section>
      <div className="d-flex justify-content-between align-items-center mb-4">
        <div>
          <h2 className="mb-1">Payment #{payment.paymentId}</h2>
          <p className="text-secondary mb-0">
            Recorded {formatDate(payment.paymentDate)}
          </p>
        </div>

        <span className={`badge text-bg-${getStatusColor(payment.status)}`}>
          {payment.status}
        </span>
      </div>

      <div className="row g-4 mb-4">
        <div className="col-lg-7">
          <div className="card h-100 shadow-sm">
            <div className="card-header bg-white">
              <h3 className="h5 mb-0">Payment Information</h3>
            </div>

            <div className="card-body">
              <div className="row g-3">
                <div className="col-md-6">
                  <p className="text-secondary mb-1">Invoice</p>
                  <Link to={`/invoices/${payment.invoiceId}`}>
                    Invoice #{payment.invoiceId}
                  </Link>
                </div>

                <div className="col-md-6">
                  <p className="text-secondary mb-1">Payment Date</p>
                  <p className="mb-0">{formatDate(payment.paymentDate)}</p>
                </div>

                <div className="col-md-6">
                  <p className="text-secondary mb-1">Payment Method</p>
                  <p className="mb-0">{payment.paymentMethod}</p>
                </div>

                <div className="col-md-6">
                  <p className="text-secondary mb-1">
                    Transaction Reference
                  </p>
                  <p className="mb-0">
                    {payment.transactionReference || "Not available"}
                  </p>
                </div>

                <div className="col-12">
                  <p className="text-secondary mb-1">Notes</p>
                  <p className="mb-0">
                    {payment.notes || "No notes provided"}
                  </p>
                </div>
              </div>
            </div>
          </div>
        </div>

        <div className="col-lg-5">
          <div className="card h-100 shadow-sm">
            <div className="card-header bg-white">
              <h3 className="h5 mb-0">Amount Summary</h3>
            </div>

            <div className="card-body d-flex flex-column justify-content-center">
              <p className="text-secondary mb-1">This Payment</p>
              <p className="display-6 mb-4">
                {formatAmount(payment.amount)}
              </p>

              <p className="text-secondary mb-1">
                Total Paid for Invoice #{payment.invoiceId}
              </p>
              <p className="h4 mb-0">{formatAmount(totalPaid)}</p>
            </div>
          </div>
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

export default PaymentDetails;
