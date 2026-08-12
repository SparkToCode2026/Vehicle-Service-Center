import api from "./api";

// Get all active branches from the backend
export function getActiveBranches() {
    return api.get("/Branch/GetActive");
}
