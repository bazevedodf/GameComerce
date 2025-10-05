export interface PedidoResponse {
  transactionId: string;
  qrCodeImage: string;
  pixCode: string;
  expirationTime: string;
  status: string
}