import axios from "axios";
import { classifyHttpError } from "../utils/httpErrors.js";

const api = axios.create({
  baseURL:
    import.meta.env?.VITE_API_BASE_URL || "http://localhost:5248",
  headers: {
    "Content-Type": "application/json",
  },
});

// Add the JWT token to every request
api.interceptors.request.use(
  (config) => {
    const token = typeof localStorage !== "undefined"
      ? localStorage.getItem("accessToken")
      : null;

    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }

    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

api.interceptors.response.use(
  (response) => response,
  (error) => {
    const errorType = classifyHttpError(error.response?.status);
    const requestUrl = error.config?.url || "";
    const isIncorrectCurrentPassword =
      requestUrl.includes("/User/ChangePassword/") &&
      error.response?.data === "Current password is incorrect";

    if (
      errorType === "unauthorized" &&
      !requestUrl.includes("/User/Login") &&
      !isIncorrectCurrentPassword &&
      typeof window !== "undefined"
    ) {
      localStorage.removeItem("accessToken");
      localStorage.removeItem("authUser");
      localStorage.removeItem("tokenExpiresAt");

      if (window.location.pathname !== "/login") {
        window.location.assign("/login?reason=session-expired");
      }
    }

    if (errorType === "forbidden" && typeof window !== "undefined") {
      sessionStorage.setItem(
        "forbiddenMessage",
        "You are signed in, but your role does not allow this action."
      );

      if (window.location.pathname !== "/unauthorized") {
        window.location.assign("/unauthorized");
      }
    }

    return Promise.reject(error);
  }
);

export default api;
