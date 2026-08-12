import api from "./api";

// Get all service types from the backend
export function getServiceTypes() {
    return api.get("/ServiceType");
}
