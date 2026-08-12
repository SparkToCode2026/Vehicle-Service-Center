import api from "./api";

// Get all active branches from the backend
export function getActiveBranches() {
    return api.get("/Branch/GetActive");
}

// Get a branch by its ID
export function getBranchById(branchId) {
    return api.get(`/Branch/GetById/${branchId}`);
}

export function getBranches() {
  return api.get("/Branch/GetAll");
}

export function getBranchesWithMechanics() {
  return api.get("/Branch/GetAllWithMechanics");
}

export function createBranch(branchData) {
  return api.post("/Branch/AddBranch", branchData);
}

export function updateBranch(id, branchData) {
  return api.put(`/Branch/Update/${id}`, branchData);
}

export function changeBranchStatus(id, isActive) {
  return api.patch(`/Branch/ChangeStatus/${id}`, null, {
    params: { isActive },
  });
}

export function deleteBranch(id) {
  return api.delete(`/Branch/Delete/${id}`);
}

export function sortBranches(descending = false) {
  return api.get("/Branch/SortByName", { params: { descending } });
}

export function getBranchStatusSummary() {
  return api.get("/Branch/CountByStatus");
}
