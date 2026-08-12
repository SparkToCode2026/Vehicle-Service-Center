import api from "./api";

export function getAllInvoices() {
  return api.get("/Invoice/GetAll");
}

export function getInvoiceById(invoiceId) {
  return api.get(`/Invoice/GetById/${invoiceId}`);
}

export function getInvoiceByServiceOrderId(serviceOrderId) {
  return api.get(
    `/Invoice/GetByServiceOrderId/${serviceOrderId}`
  );
}

export function createInvoice(invoiceData) {
  return api.post("/Invoice/AddInvoice", invoiceData);
}

export function updateInvoice(id, invoiceData) {
  return api.put(`/Invoice/Update/${id}`, invoiceData);
}

export function changeInvoiceStatus(id, status) {
  return api.patch(`/Invoice/ChangeStatus/${id}`, null, {
    params: { status },
  });
}

export function deleteInvoice(id) {
  return api.delete(`/Invoice/Delete/${id}`);
}

export function filterInvoices(filters) {
  return api.get("/Invoice/Filter", { params: filters });
}

export function sortInvoicesByTotal(descending = true) {
  return api.get("/Invoice/SortByTotalAmount", {
    params: { descending },
  });
}

export function getInvoiceRevenueSummary() {
  return api.get("/Invoice/RevenueSummary");
}
