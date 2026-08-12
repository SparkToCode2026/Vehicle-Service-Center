import { useCallback, useState } from "react";
import EntityManager from "../../components/shared/EntityManager";
import {
  changePaymentStatus, createPayment, deletePayment, filterPayments,
  getAllPayments, getTotalPaidForInvoice, sortPaymentsByDate, updatePayment,
} from "../../api/paymentApi";
import { getApiErrorMessage } from "../../utils/httpErrors";

const initialValues = { invoiceId: "", amount: 0, paymentDate: "", paymentMethod: "Card", transactionReference: "", status: "Pending", notes: "" };
const toPayload = (values) => ({ ...values, invoiceId: Number(values.invoiceId), amount: Number(values.amount), paymentDate: values.paymentDate || new Date().toISOString() });

function PaymentManagement() {
  const [status, setStatus] = useState("");
  const [invoiceTotal, setInvoiceTotal] = useState(null);
  const loadItems = useCallback(() => getAllPayments(), []);

  return <EntityManager
    title="Payment Management" description="Record and maintain payments with filters, status, sorting, and invoice totals."
    idKey="paymentId" loadItems={loadItems} createItem={createPayment} updateItem={updatePayment} deleteItem={deletePayment}
    initialValues={initialValues} prepareCreate={toPayload} prepareUpdate={toPayload}
    normalizeForEdit={(item) => ({ ...item, paymentDate: item.paymentDate?.slice(0, 16) })}
    fields={[
      { name: "invoiceId", label: "Invoice ID", type: "number", min: 1, required: true }, { name: "amount", label: "Amount", type: "number", min: 0.01, step: "0.01", required: true },
      { name: "paymentDate", label: "Payment date", type: "datetime-local" }, { name: "paymentMethod", label: "Method", type: "select", required: true, options: ["Cash", "Card", "Bank Transfer", "Online"].map((value) => ({ value, label: value })) },
      { name: "transactionReference", label: "Transaction reference" }, { name: "status", label: "Status", type: "select", required: true, options: ["Pending", "Completed", "Failed", "Refunded"].map((value) => ({ value, label: value })) },
      { name: "notes", label: "Notes", type: "textarea", columnClass: "col-12" },
    ]}
    columns={[
      { key: "paymentId", label: "ID" }, { key: "invoiceId", label: "Invoice" }, { key: "amount", label: "Amount" },
      { key: "paymentDate", label: "Date" }, { key: "paymentMethod", label: "Method" }, { key: "status", label: "Status" },
    ]}
    actions={({ item, reload, setError }) => <button className="btn btn-outline-warning btn-sm" type="button" onClick={async () => {
      const nextStatus = window.prompt("New payment status:", item.status);
      if (!nextStatus) return;
      try { await changePaymentStatus(item.paymentId, nextStatus); reload(); } catch (error) { setError(getApiErrorMessage(error)); }
    }}>Change status</button>}
    toolbar={({ setItems, reload, setError }) => <div className="card card-body shadow-sm mb-4"><div className="row g-2 align-items-end"><div className="col-md-4"><label className="form-label" htmlFor="payment-status-filter">Status</label><input id="payment-status-filter" className="form-control" value={status} onChange={(event) => setStatus(event.target.value)} /></div><div className="col-md-8 d-flex flex-wrap gap-2"><button className="btn btn-outline-primary" disabled={!status.trim()} type="button" onClick={async () => { try { setItems((await filterPayments({ status })).data); } catch (error) { setError(getApiErrorMessage(error)); } }}>Filter</button><button className="btn btn-outline-secondary" type="button" onClick={async () => { try { setItems((await sortPaymentsByDate(true)).data); } catch (error) { setError(getApiErrorMessage(error)); } }}>Newest</button><button className="btn btn-outline-info" type="button" onClick={async () => { const id = window.prompt("Invoice ID:"); if (!id) return; try { setInvoiceTotal((await getTotalPaidForInvoice(id)).data); } catch (error) { setError(getApiErrorMessage(error)); } }}>Invoice total</button><button className="btn btn-outline-dark" type="button" onClick={reload}>Reset</button></div></div>{invoiceTotal && <div className="alert alert-info mt-3 mb-0">Invoice #{invoiceTotal.invoiceId} paid total: {Number(invoiceTotal.totalPaid).toFixed(2)}</div>}</div>}
  />;
}

export default PaymentManagement;
