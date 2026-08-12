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