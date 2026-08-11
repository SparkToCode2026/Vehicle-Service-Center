import api from "./api";

export function getAllServiceOrders() {
  return api.get("/api/ServiceOrder");
}

export function getServiceOrderById(serviceOrderId) {
  return api.get(`/api/ServiceOrder/${serviceOrderId}`);
}

export function filterServiceOrders(filters) {
  return api.get("/api/ServiceOrder/filter", {
    params: filters,
  });
}

export function getServiceOrderSummary() {
  return api.get("/api/ServiceOrder/summary");
}

export function deleteServiceOrder(serviceOrderId) {
  return api.delete(`/api/ServiceOrder/${serviceOrderId}`);
}
