import { useEffect, useState } from "react";
import { Link } from "react-router";
import { getAllInvoices } from "../../api/invoiceApi";
import { getAllPayments } from "../../api/paymentApi";
import { getApiErrorMessage } from "../../utils/httpErrors";

function formatAmount(amount) {
  return Number(amount || 0).toFixed(2);
}

function MechanicBilling() {
  const [invoices, setInvoices] = useState([]);
  const [payments, setPayments] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  useEffect(() => {
    async function loadBilling() {
      try {
        setLoading(true);
        setError("");
        const [invoiceResponse, paymentResponse] = await Promise.all([
          getAllInvoices(),
          getAllPayments(),
        ]);
        setInvoices(invoiceResponse.data);
        setPayments(paymentResponse.data);
      } catch (requestError) {
        setError(getApiErrorMessage(requestError, "Could not load billing records."));
      } finally {
        setLoading(false);
      }
    }

    loadBilling();
  }, []);

  return (
    <section>
      <div className="mb-4">
        <h2 className="mb-1">Billing Records</h2>
        <p className="text-secondary mb-0">Read-only invoices and payments for your assigned service orders.</p>
      </div>

      {error && <div className="alert alert-danger">{error}</div>}
      {loading ? (
        <div className="text-center py-5"><div className="spinner-border text-primary" /></div>
      ) : (
        <div className="row g-4">
          <div className="col-lg-6">
            <div className="card shadow-sm h-100">
              <div className="card-header bg-white"><h3 className="h5 mb-0">Assigned invoices</h3></div>
              <div className="card-body p-0">
                {invoices.length === 0 ? <p className="text-secondary p-3 mb-0">No invoices found.</p> : (
                  <div className="table-responsive"><table className="table table-hover mb-0"><thead><tr><th>Invoice</th><th>Total</th><th>Status</th><th /></tr></thead><tbody>{invoices.map((invoice) => <tr key={invoice.invoiceId}><td>{invoice.invoiceNumber}</td><td>{formatAmount(invoice.totalAmount)}</td><td>{invoice.status}</td><td><Link to={`/invoices/${invoice.invoiceId}`}>Details</Link></td></tr>)}</tbody></table></div>
                )}
              </div>
            </div>
          </div>
          <div className="col-lg-6">
            <div className="card shadow-sm h-100">
              <div className="card-header bg-white"><h3 className="h5 mb-0">Related payments</h3></div>
              <div className="card-body p-0">
                {payments.length === 0 ? <p className="text-secondary p-3 mb-0">No payments found.</p> : (
                  <div className="table-responsive"><table className="table table-hover mb-0"><thead><tr><th>Payment</th><th>Invoice</th><th>Amount</th><th>Status</th><th /></tr></thead><tbody>{payments.map((payment) => <tr key={payment.paymentId}><td>#{payment.paymentId}</td><td>#{payment.invoiceId}</td><td>{formatAmount(payment.amount)}</td><td>{payment.status}</td><td><Link to={`/payments/${payment.paymentId}`}>Details</Link></td></tr>)}</tbody></table></div>
                )}
              </div>
            </div>
          </div>
        </div>
      )}
    </section>
  );
}

export default MechanicBilling;
