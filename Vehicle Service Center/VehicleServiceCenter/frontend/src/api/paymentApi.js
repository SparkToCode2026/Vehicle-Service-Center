import api from "./api";

export function getAllPayments() {
  return api.get("/Payment/GetAll");
}

export function getPaymentById(paymentId) {
  return api.get(`/Payment/GetById/${paymentId}`);
}

export function getTotalPaidForInvoice(invoiceId) {
  return api.get(`/Payment/GetTotalByInvoice/${invoiceId}`);
}

export function createPayment(paymentData) {
  return api.post("/Payment/AddPayment", paymentData);
}

export function updatePayment(id, paymentData) {
  return api.put(`/Payment/Update/${id}`, paymentData);
}

export function changePaymentStatus(id, status) {
  return api.patch(`/Payment/ChangeStatus/${id}`, null, {
    params: { status },
  });
}

export function deletePayment(id) {
  return api.delete(`/Payment/Delete/${id}`);
}

export function filterPayments(filters) {
  return api.get("/Payment/Filter", { params: filters });
}

export function sortPaymentsByDate(descending = true) {
  return api.get("/Payment/SortByDate", { params: { descending } });
}
