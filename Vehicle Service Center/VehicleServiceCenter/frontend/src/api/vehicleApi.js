import api from "./api";

export function createVehicle(vehicleData) {
  return api.post("/Vehicle", vehicleData);
}

export function getAllVehicles() {
  return api.get("/Vehicle");
}

export function getVehicleById(vehicleId) {
  return api.get(`/Vehicle/${vehicleId}`);
}

export function filterVehiclesByMake(make) {
  return api.get("/Vehicle/filter", {
    params: {
      make,
    },
  });
}

export function getVehicleCountByMake() {
  return api.get("/Vehicle/summary");
}

export function updateVehicle(vehicleId, vehicleData) {
  return api.put(`/Vehicle/${vehicleId}`, vehicleData);
}

export function reassignVehicle(vehicleId, customerProfileId) {
  return api.patch(`/Vehicle/${vehicleId}/reassign`, null, {
    params: {
      customerProfileId,
    },
  });
}

export function deleteVehicle(vehicleId) {
  return api.delete(`/Vehicle/${vehicleId}`);
}
