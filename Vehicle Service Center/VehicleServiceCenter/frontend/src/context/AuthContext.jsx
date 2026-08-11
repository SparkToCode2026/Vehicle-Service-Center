import {
  createContext,
  useCallback,
  useContext,
  useEffect,
  useState,
} from "react";
import { loginUser, registerUser } from "../api/authApi";

const AuthContext = createContext(null);

function clearStoredAuthentication() {
  localStorage.removeItem("accessToken");
  localStorage.removeItem("authUser");
  localStorage.removeItem("tokenExpiresAt");
}

function getStoredAuthentication() {
  const accessToken = localStorage.getItem("accessToken");
  const storedUser = localStorage.getItem("authUser");
  const expiresAt = localStorage.getItem("tokenExpiresAt");

  const tokenHasExpired =
    !expiresAt || new Date(expiresAt).getTime() <= Date.now();

  if (!accessToken || !storedUser || tokenHasExpired) {
    clearStoredAuthentication();

    return {
      user: null,
      accessToken: null,
      expiresAt: null,
    };
  }

  try {
    return {
      user: JSON.parse(storedUser),
      accessToken,
      expiresAt,
    };
  } catch {
    clearStoredAuthentication();

    return {
      user: null,
      accessToken: null,
      expiresAt: null,
    };
  }
}

export function AuthProvider({ children }) {
  const [authentication, setAuthentication] = useState(
    getStoredAuthentication
  );

  const logout = useCallback(() => {
    clearStoredAuthentication();

    setAuthentication({
      user: null,
      accessToken: null,
      expiresAt: null,
    });
  }, []);

  useEffect(() => {
    if (!authentication.expiresAt) {
      return undefined;
    }

    const remainingTime =
      new Date(authentication.expiresAt).getTime() - Date.now();

    const logoutTimer = window.setTimeout(logout, remainingTime);

    return () => window.clearTimeout(logoutTimer);
  }, [authentication.expiresAt, logout]);

  async function login(email, password) {
    const response = await loginUser(email, password);
    const loginData = response.data;

    const loggedInUser = {
      userId: loginData.userId,
      userName: loginData.userName,
      role: loginData.role,
    };

    localStorage.setItem("accessToken", loginData.accessToken);
    localStorage.setItem("authUser", JSON.stringify(loggedInUser));
    localStorage.setItem("tokenExpiresAt", loginData.expiresAtUtc);

    setAuthentication({
      user: loggedInUser,
      accessToken: loginData.accessToken,
      expiresAt: loginData.expiresAtUtc,
    });

    return loggedInUser;
  }

  async function register(userData) {
    const response = await registerUser(userData);
    return response.data;
  }

  const contextValue = {
    user: authentication.user,
    accessToken: authentication.accessToken,
    isAuthenticated: Boolean(authentication.accessToken),
    login,
    register,
    logout,
  };

  return (
    <AuthContext.Provider value={contextValue}>
      {children}
    </AuthContext.Provider>
  );
}

// Keeping the provider and its hook together makes authentication easier to use.
// oxlint-disable-next-line react/only-export-components
export function useAuth() {
  const authentication = useContext(AuthContext);

  if (!authentication) {
    throw new Error("useAuth must be used inside AuthProvider.");
  }

  return authentication;
}
