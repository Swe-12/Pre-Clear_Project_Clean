import http, { setAuthToken } from '../../api/http';

export async function signUp(payload) {
  const resp = await http.post('/auth/signup', payload);
  return resp.data;
}

export async function signIn(payload) {
  const resp = await http.post('/auth/signin', payload);
  const data = resp.data;
  if (data?.token) setAuthToken(data.token);
  try { if (data?.id != null) localStorage.setItem('pc_userId', String(data.id)); } catch {}
  return data;
}
