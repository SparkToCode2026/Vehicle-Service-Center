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
