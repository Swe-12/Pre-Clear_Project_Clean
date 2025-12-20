import http from '../../api/http';

export async function sendMessage(shipmentId, message, senderId = null) {
  const resp = await http.post(`/chat/shipments/${shipmentId}/messages`, { message, senderId });
  return resp.data;
}
