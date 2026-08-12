import api from "./api";

// Get all active branches from the backend
export function getActiveBranches() {
    return api.get("/Branch/GetActive");
}

// Get a branch by its ID
export function getBranchById(branchId) {
    return api.get(`/Branch/GetById/${branchId}`);
}