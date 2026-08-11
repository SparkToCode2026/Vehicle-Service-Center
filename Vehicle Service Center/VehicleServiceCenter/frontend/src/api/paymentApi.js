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
