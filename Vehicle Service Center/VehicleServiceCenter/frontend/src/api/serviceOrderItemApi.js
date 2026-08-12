import api from "./api.js";

export function getServiceOrderItems() {
  return api.get("/api/ServiceOrderItem");
}

export function getServiceOrderItem(id) {
  return api.get(`/api/ServiceOrderItem/${id}`);
}

export function createServiceOrderItem(itemData) {
  return api.post("/api/ServiceOrderItem", itemData);
}

export function updateServiceOrderItem(id, itemData) {
  return api.put(`/api/ServiceOrderItem/${id}`, itemData);
}

export function updateServiceOrderItemQuantity(id, quantity) {
  return api.patch(`/api/ServiceOrderItem/${id}/quantity`, quantity);
}

export function deleteServiceOrderItem(id) {
  return api.delete(`/api/ServiceOrderItem/${id}`);
}

export function filterServiceOrderItems(filters) {
  return api.get("/api/ServiceOrderItem/filter", { params: filters });
}

export function getServiceOrderTotal(serviceOrderId) {
  return api.get(`/api/ServiceOrderItem/total/${serviceOrderId}`);
}
