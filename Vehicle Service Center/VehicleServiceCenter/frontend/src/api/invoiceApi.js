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
