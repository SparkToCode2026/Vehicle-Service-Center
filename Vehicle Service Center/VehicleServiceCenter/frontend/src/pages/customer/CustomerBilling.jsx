import { useCallback, useEffect, useState } from "react";
import { Link } from "react-router";
import { getAllInvoices } from "../../api/invoiceApi";
import { createPayment, getAllPayments } from "../../api/paymentApi";
import { getApiErrorMessage } from "../../utils/httpErrors";

const blankPayment = {
  invoiceId: "",
  amount: "",
  paymentMethod: "Card",
  transactionReference: "",
  notes: "",
};

function CustomerBilling() {
  const [invoices, setInvoices] = useState([]);
  const [payments, setPayments] = useState([]);
  const [form, setForm] = useState(blankPayment);
  const [loading, setLoading] = useState(true);
  const [saving, setSaving] = useState(false);
  const [error, setError] = useState("");
  const [message, setMessage] = useState("");

  const load = useCallback(async () => {
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
      setError(getApiErrorMessage(requestError, "Could not load billing information."));
    } finally {
      setLoading(false);
    }
  }, []);

  useEffect(() => { load(); }, [load]);

  async function submitPayment(event) {
    event.preventDefault();
    try {
      setSaving(true);
      setError("");
      setMessage("");
      await createPayment({
        ...form,
        invoiceId: Number(form.invoiceId),
        amount: Number(form.amount),
        paymentDate: new Date().toISOString(),
        transactionReference: form.transactionReference.trim() || null,
        notes: form.notes.trim() || null,
        status: "Pending",
      });
      setForm(blankPayment);
      setMessage("Payment recorded successfully.");
      await load();
    } catch (requestError) {
      setError(getApiErrorMessage(requestError, "Could not record the payment."));
    } finally {
      setSaving(false);
    }
  }

  return (
    <section>
      <div className="mb-4">
        <h2 className="mb-1">Billing</h2>
        <p className="text-secondary mb-0">View your invoices, record a payment, and review payment history.</p>
      </div>
      {error && <div className="alert alert-danger">{error}</div>}
      {message && <div className="alert alert-success">{message}</div>}

      <div className="card shadow-sm mb-4">
        <div className="card-header bg-white"><h3 className="h5 mb-0">Record payment</h3></div>
        <form className="card-body" onSubmit={submitPayment}>
          <div className="row g-3">
            <div className="col-md-4"><label className="form-label" htmlFor="billing-invoice">Invoice</label><select id="billing-invoice" className="form-select" required value={form.invoiceId} onChange={(event) => setForm({ ...form, invoiceId: event.target.value })}><option value="">Select...</option>{invoices.map((invoice) => <option key={invoice.invoiceId} value={invoice.invoiceId}>{invoice.invoiceNumber} · {Number(invoice.totalAmount).toFixed(2)}</option>)}</select></div>
            <div className="col-md-4"><label className="form-label" htmlFor="billing-amount">Amount</label><input id="billing-amount" className="form-control" type="number" min="0.01" step="0.01" required value={form.amount} onChange={(event) => setForm({ ...form, amount: event.target.value })} /></div>
            <div className="col-md-4"><label className="form-label" htmlFor="billing-method">Method</label><select id="billing-method" className="form-select" value={form.paymentMethod} onChange={(event) => setForm({ ...form, paymentMethod: event.target.value })}><option>Card</option><option>Cash</option><option>Bank Transfer</option><option>Online</option></select></div>
            <div className="col-md-6"><label className="form-label" htmlFor="billing-reference">Transaction reference</label><input id="billing-reference" className="form-control" maxLength="100" value={form.transactionReference} onChange={(event) => setForm({ ...form, transactionReference: event.target.value })} /></div>
            <div className="col-md-6"><label className="form-label" htmlFor="billing-notes">Notes</label><input id="billing-notes" className="form-control" maxLength="500" value={form.notes} onChange={(event) => setForm({ ...form, notes: event.target.value })} /></div>
          </div>
          <button className="btn btn-primary mt-3" type="submit" disabled={saving || invoices.length === 0}>{saving ? "Recording..." : "Record payment"}</button>
        </form>
      </div>

      {loading ? <div className="text-center py-5"><div className="spinner-border text-primary" /></div> : (
        <div className="row g-4">
          <div className="col-lg-6"><div className="card shadow-sm h-100"><div className="card-header bg-white"><h3 className="h5 mb-0">My invoices</h3></div><div className="card-body p-0">{invoices.length === 0 ? <p className="text-secondary p-3 mb-0">No invoices found.</p> : <div className="table-responsive"><table className="table table-hover mb-0"><thead><tr><th>Invoice</th><th>Total</th><th>Status</th><th /></tr></thead><tbody>{invoices.map((invoice) => <tr key={invoice.invoiceId}><td>{invoice.invoiceNumber}</td><td>{Number(invoice.totalAmount).toFixed(2)}</td><td>{invoice.status}</td><td><Link to={`/invoices/${invoice.invoiceId}`}>Details</Link></td></tr>)}</tbody></table></div>}</div></div></div>
          <div className="col-lg-6"><div className="card shadow-sm h-100"><div className="card-header bg-white"><h3 className="h5 mb-0">My payments</h3></div><div className="card-body p-0">{payments.length === 0 ? <p className="text-secondary p-3 mb-0">No payments found.</p> : <div className="table-responsive"><table className="table table-hover mb-0"><thead><tr><th>Payment</th><th>Amount</th><th>Status</th><th /></tr></thead><tbody>{payments.map((payment) => <tr key={payment.paymentId}><td>#{payment.paymentId}</td><td>{Number(payment.amount).toFixed(2)}</td><td>{payment.status}</td><td><Link to={`/payments/${payment.paymentId}`}>Details</Link></td></tr>)}</tbody></table></div>}</div></div></div>
        </div>
      )}
    </section>
  );
}

export default CustomerBilling;
