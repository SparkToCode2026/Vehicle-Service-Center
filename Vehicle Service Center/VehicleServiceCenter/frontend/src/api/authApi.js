import api from "./api";

export function loginUser(email, password) {
  return api.post("/User/Login", {
    email,
    password,
  });
}

export function registerUser(userData) {
  return api.post("/User/RegisterUser", userData);
}

export function getUsers() {
  return api.get("/User/GetAll");
}

export function getUserById(id) {
  return api.get(`/User/GetById/${id}`);
}

export function updateUser(id, userData) {
  return api.put(`/User/Update/${id}`, userData);
}

export function changeUserStatus(id, isActive) {
  return api.patch(`/User/ChangeStatus/${id}`, null, {
    params: { isActive },
  });
}

export function changePassword(id, currentPassword, newPassword) {
  return api.put(`/User/ChangePassword/${id}`, null, {
    params: { currentPassword, newPassword },
  });
}

export function filterUsers(role, isActive) {
  return api.get("/User/FilterByRole", { params: { role, isActive } });
}

export function getUserRoleSummary() {
  return api.get("/User/GetRoleSummary");
}

export function deleteUser(id) {
  return api.delete(`/User/Delete/${id}`);
}
