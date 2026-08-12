import api from "./api";

// Get all service types from the backend
export function getServiceTypes() {
    return api.get("/ServiceType");
}

// Get only active service types
export function getActiveServiceTypes() {
    return api.get("/ServiceType/filter", {
        params: {
            isActive: true,
        },
    });
}

export function getServiceTypeById(id) {
  return api.get(`/ServiceType/${id}`);
}

export function createServiceType(serviceTypeData) {
  return api.post("/ServiceType", serviceTypeData);
}

export function updateServiceType(id, serviceTypeData) {
  return api.put(`/ServiceType/${id}`, serviceTypeData);
}

export function changeServiceTypeStatus(id, isActive) {
  return api.patch(`/ServiceType/${id}/status`, null, {
    params: { isActive },
  });
}

export function deleteServiceType(id) {
  return api.delete(`/ServiceType/${id}`);
}

export function filterServiceTypes(isActive) {
  return api.get("/ServiceType/filter", { params: { isActive } });
}

export function getServiceTypeRevenue() {
  return api.get("/ServiceType/revenue");
}
