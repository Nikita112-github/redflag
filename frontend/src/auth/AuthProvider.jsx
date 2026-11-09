import { useCallback, useState } from 'react';
import { api } from '../api/client';
import { AuthContext } from './AuthContext';

const STORAGE_KEY = 'redflag_auth';

function readStoredAuth() {
  try {
    const raw = localStorage.getItem(STORAGE_KEY);
    if (!raw) return null;
    const stored = JSON.parse(raw);
    if (new Date(stored.expiresAtUtc) <= new Date()) {
      localStorage.removeItem(STORAGE_KEY);
      return null;
    }
    return stored;
  } catch {
    return null;
  }
}

export default function AuthProvider({ children }) {
  const [auth, setAuth] = useState(readStoredAuth);

  const login = useCallback(async (email, password) => {
    const data = await api.login(email, password);
    localStorage.setItem(STORAGE_KEY, JSON.stringify(data));
    setAuth(data);
    return data;
  }, []);

  const logout = useCallback(() => {
    localStorage.removeItem(STORAGE_KEY);
    setAuth(null);
  }, []);

  const value = {
    user: auth ? { email: auth.email, role: auth.role } : null,
    token: auth ? auth.token : null,
    isAdmin: auth ? auth.role === 'Admin' : false,
    login,
    logout,
  };

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
}