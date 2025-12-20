import http, { setAuthToken, clearAuthToken } from './http';

export async function signUp(payload) {
  const resp = await http.post('/auth/signup', payload);
  return resp.data;
}

export async function signIn(payload) {
  const resp = await http.post('/auth/signin', payload);
  const data = resp.data;
  // Store JWT token centrally for later requests
  if (data?.token) {
    setAuthToken(data.token);
  }
  // Optionally keep userId for legacy code paths
  try {
    if (data?.id != null) {
      localStorage.setItem('pc_userId', String(data.id));
    }
  } catch { /* ignore */ }
  return data;
}

export function signOut() {
  try { localStorage.removeItem('pc_userId'); } catch { /* ignore */ }
  clearAuthToken();
}

export async function getProfile() {
  const resp = await http.get('/profiles');
  return resp.data;
}

export async function updateProfile(payload) {
  const resp = await http.put('/profiles', payload);
  return resp.data;
}

export async function changePassword(payload) {
  const resp = await http.post('/auth/change-password', payload);
  return resp.data;
}
