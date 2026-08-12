import api from "./api";

export function getAllSpareParts() {
  return api.get("/SparePart");
}

export function getSparePartById(sparePartId) {
  return api.get(`/SparePart/${sparePartId}`);
}

export function filterSparePartsByAvailability(isAvailable) {
  return api.get("/SparePart/filter", {
    params: {
      isAvailable,
    },
  });
}

export function getSparePartsSortedByPrice() {
  return api.get("/SparePart/sort");
}

export function createSparePart(sparePartData) {
  return api.post("/SparePart", sparePartData);
}

export function updateSparePart(id, sparePartData) {
  return api.put(`/SparePart/${id}`, sparePartData);
}

export function updateSparePartStock(id, quantity) {
  return api.patch(`/SparePart/${id}/stock`, null, {
    params: { quantity },
  });
}

export function deleteSparePart(id) {
  return api.delete(`/SparePart/${id}`);
}
