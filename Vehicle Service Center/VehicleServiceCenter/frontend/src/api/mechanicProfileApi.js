import api from "./api";

export function getMechanicProfileByUserId(userId) {
    return api.get("/MechanicProfile/GetByUserId", {
        params: {
            userId,
        },
    });
}

export function updateMechanicAvailability(
    mechanicProfileId,
    isAvailable
) {
    return api.patch("/MechanicProfile/UpdateAvailability", null, {
        params: {
            id: mechanicProfileId,
            isAvailable,
        },
    });
}

export function getMechanicProfiles() {
  return api.get("/MechanicProfile/GetAllMechanicProfiles");
}

export function getMechanicProfile(id) {
  return api.get("/MechanicProfile/GetMechanicProfile", { params: { id } });
}

export function createMechanicProfile(profileData) {
  return api.post("/MechanicProfile/AddMechanicProfile", profileData);
}

export function updateMechanicProfile(id, profileData) {
  return api.put("/MechanicProfile/UpdateMechanicProfile", profileData, {
    params: { id },
  });
}

export function deleteMechanicProfile(id) {
  return api.delete("/MechanicProfile/RemoveMechanicProfile", {
    params: { id },
  });
}

export function getMechanicsByBranch(branchId) {
  return api.get("/MechanicProfile/GetByBranchId", {
    params: { branchId },
  });
}

export function sortMechanicsByExperience(descending = true) {
  return api.get("/MechanicProfile/GetSortedByExperience", {
    params: { descending },
  });
}
