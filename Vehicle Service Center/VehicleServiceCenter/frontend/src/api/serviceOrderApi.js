import api from "./api";

export function getAllServiceOrders() {
  return api.get("/api/ServiceOrder");
}

export function getServiceOrderById(serviceOrderId) {
  return api.get(`/api/ServiceOrder/${serviceOrderId}`);
}

export function getServiceOrdersByMechanic(mechanicProfileId) {
  return api.get(`/api/ServiceOrder/mechanic/${mechanicProfileId}`);
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

export function createServiceOrder(orderData) {
  return api.post("/api/ServiceOrder", orderData);
}

export function updateServiceOrder(id, orderData) {
  return api.put(`/api/ServiceOrder/${id}`, orderData);
}

export function changeServiceOrderStatus(id, newStatus) {
  return api.patch(`/api/ServiceOrder/${id}/status`, JSON.stringify(newStatus));
}
