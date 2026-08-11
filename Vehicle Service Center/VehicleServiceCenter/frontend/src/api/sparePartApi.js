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
