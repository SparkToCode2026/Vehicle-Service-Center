import api from "./api";

export function createCustomerProfile(profileData) {
  return api.post("/CustomerProfile/AddCustomerProfile", profileData);
}

export function getCustomerProfileById(customerProfileId) {
  return api.get("/CustomerProfile/GetCustomerProfile", {
    params: {
      id: customerProfileId,
    },
  });
}

export function getCustomerProfileByUserId(userId) {
  return api.get("/CustomerProfile/GetByUserId", {
    params: {
      userId,
    },
  });
}

export function getAllCustomerProfiles() {
  return api.get("/CustomerProfile/GetAllCustomerProfiles");
}

export function filterCustomerProfiles(filters) {
  return api.get("/CustomerProfile/Filter", {
    params: filters,
  });
}

export function getCustomerProfilesSortedByCreatedAt(
  descending = true
) {
  return api.get("/CustomerProfile/GetSortedByCreatedAt", {
    params: {
      descending,
    },
  });
}

export function updateCustomerProfile(customerProfileId, profileData) {
  return api.put(
    "/CustomerProfile/UpdateCustomerProfile",
    profileData,
    {
      params: {
        id: customerProfileId,
      },
    }
  );
}

export function updateCustomerAddress(customerProfileId, newAddress) {
  return api.patch("/CustomerProfile/UpdateAddress", null, {
    params: {
      id: customerProfileId,
      newAddress,
    },
  });
}

export function deleteCustomerProfile(customerProfileId) {
  return api.delete("/CustomerProfile/RemoveCustomerProfile", {
    params: {
      id: customerProfileId,
    },
  });
}
