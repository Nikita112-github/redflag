const BASE_URL = import.meta.env.VITE_API_URL ?? 'http://localhost:5209';

export class ApiError extends Error {
  constructor(message, status) {
    super(message);
    this.name = 'ApiError';
    this.status = status;
  }
}

function extractMessage(data, status) {
  if (data?.error) {
    return data.error;
  }
  if (data?.errors) {
    const messages = Object.values(data.errors).flat();
    if (messages.length > 0) {
      return messages.join(' ');
    }
  }
  if (status === 401) return 'You need to log in.';
  if (status === 403) return 'You do not have permission to do this.';
  if (status === 404) return 'Not found.';
  return data?.title ?? `Request failed (${status}).`;
}

async function request(path, { method = 'GET', body, token } = {}) {
  const headers = {};
  if (body !== undefined) {
    headers['Content-Type'] = 'application/json';
  }
  if (token) {
    headers.Authorization = `Bearer ${token}`;
  }

  let response;
  try {
    response = await fetch(`${BASE_URL}${path}`, {
      method,
      headers,
      body: body !== undefined ? JSON.stringify(body) : undefined,
    });
  } catch {
    throw new ApiError('Cannot reach the server. Is the API running?', 0);
  }

  let data = null;
  const text = await response.text();
  if (text) {
    try {
      data = JSON.parse(text);
    } catch {
      data = null;
    }
  }

  if (!response.ok) {
    throw new ApiError(extractMessage(data, response.status), response.status);
  }
  return data;
}

export const api = {
  analyze: (text) => request('/api/analyze', { method: 'POST', body: { text } }),
  getReport: (id) => request(`/api/reports/${id}`),
  register: (email, password) =>
    request('/api/auth/register', { method: 'POST', body: { email, password } }),
  login: (email, password) =>
    request('/api/auth/login', { method: 'POST', body: { email, password } }),
  getStats: (token) => request('/api/admin/stats', { token }),
  getLatestReports: (token, count = 10) =>
    request(`/api/admin/reports/latest?count=${count}`, { token }),
};