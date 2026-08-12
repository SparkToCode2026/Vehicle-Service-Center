import { useCallback, useState } from "react";
import EntityManager from "../../components/shared/EntityManager";
import {
  changeInvoiceStatus, createInvoice, deleteInvoice, filterInvoices,
  getAllInvoices, getInvoiceRevenueSummary, sortInvoicesByTotal, updateInvoice,
} from "../../api/invoiceApi";
import { getApiErrorMessage } from "../../utils/httpErrors";

const initialValues = { serviceOrderId: "", invoiceNumber: "", issueDate: "", dueDate: "", subtotal: 0, taxAmount: 0, discountAmount: 0, totalAmount: 0, status: "Pending", notes: "" };
const toPayload = (values) => ({ ...values, serviceOrderId: Number(values.serviceOrderId), subtotal: Number(values.subtotal), taxAmount: Number(values.taxAmount), discountAmount: Number(values.discountAmount), totalAmount: Number(values.totalAmount || 0), issueDate: values.issueDate || new Date().toISOString(), dueDate: values.dueDate || null });

function InvoiceManagement() {
  const [status, setStatus] = useState("");
  const [summary, setSummary] = useState([]);
  const loadItems = useCallback(() => getAllInvoices(), []);

  return <EntityManager
    title="Invoice Management" description="Create, update, filter, sort, summarize, and delete invoices."
    idKey="invoiceId" loadItems={loadItems} createItem={createInvoice} updateItem={updateInvoice} deleteItem={deleteInvoice}
    initialValues={initialValues} prepareCreate={toPayload} prepareUpdate={toPayload}
    fields={[
      { name: "serviceOrderId", label: "Service order ID", type: "number", min: 1, required: true },
      { name: "invoiceNumber", label: "Invoice number", required: true }, { name: "issueDate", label: "Issue date", type: "datetime-local" },
      { name: "dueDate", label: "Due date", type: "datetime-local" }, { name: "subtotal", label: "Subtotal", type: "number", min: 0, step: "0.01", required: true },
      { name: "taxAmount", label: "Tax", type: "number", min: 0, step: "0.01", required: true }, { name: "discountAmount", label: "Discount", type: "number", min: 0, step: "0.01", required: true },
      { name: "status", label: "Status", type: "select", required: true, options: ["Pending", "Issued", "Paid", "Cancelled"].map((value) => ({ value, label: value })) },
      { name: "notes", label: "Notes", type: "textarea", columnClass: "col-12" },
    ]}
    normalizeForEdit={(item) => ({ ...item, issueDate: item.issueDate?.slice(0, 16), dueDate: item.dueDate?.slice(0, 16) || "" })}
    columns={[
      { key: "invoiceId", label: "ID" }, { key: "invoiceNumber", label: "Invoice" }, { key: "serviceOrderId", label: "Order" },
      { key: "issueDate", label: "Issued" }, { key: "totalAmount", label: "Total" }, { key: "status", label: "Status" },
    ]}
    actions={({ item, reload, setError }) => <button className="btn btn-outline-warning btn-sm" type="button" onClick={async () => {
      const nextStatus = window.prompt("New invoice status:", item.status);
      if (!nextStatus) return;
      try { await changeInvoiceStatus(item.invoiceId, nextStatus); reload(); } catch (error) { setError(getApiErrorMessage(error)); }
    }}>Change status</button>}
    toolbar={({ setItems, reload, setError }) => <div className="card card-body shadow-sm mb-4"><div className="row g-2 align-items-end"><div className="col-md-4"><label className="form-label" htmlFor="invoice-status-filter">Status</label><input id="invoice-status-filter" className="form-control" value={status} onChange={(event) => setStatus(event.target.value)} /></div><div className="col-md-8 d-flex flex-wrap gap-2"><button className="btn btn-outline-primary" disabled={!status.trim()} type="button" onClick={async () => { try { setItems((await filterInvoices({ status })).data); } catch (error) { setError(getApiErrorMessage(error)); } }}>Filter</button><button className="btn btn-outline-secondary" type="button" onClick={async () => { try { setItems((await sortInvoicesByTotal(true)).data); } catch (error) { setError(getApiErrorMessage(error)); } }}>Highest total</button><button className="btn btn-outline-info" type="button" onClick={async () => { try { setSummary((await getInvoiceRevenueSummary()).data); } catch (error) { setError(getApiErrorMessage(error)); } }}>Revenue</button><button className="btn btn-outline-dark" type="button" onClick={reload}>Reset</button></div></div>{summary.length > 0 && <div className="d-flex flex-wrap gap-2 mt-3">{summary.map((item) => <span className="badge text-bg-info" key={item.status}>{item.status}: {Number(item.totalAmount).toFixed(2)} ({item.count})</span>)}</div>}</div>}
  />;
}

export default InvoiceManagement;
