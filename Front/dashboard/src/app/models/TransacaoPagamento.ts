export interface TransacaoPagamento {
  id: number;
  pedidoId: number;

  // Dados enviados ao gateway
  amount: number;
  paymentMethod: string;

  // Customer
  customerName?: string;
  customerEmail?: string;
  customerPhone?: string;
  customerDocument?: string;

  // Address
  zipCode?: string;
  street?: string;
  number?: string;
  neighborhood?: string;
  city?: string;
  state?: string;
  country: string;

  // Resposta do gateway
  transactionId: string;
  status?: string; // "pending", "paid", "expired"
  pixCode?: string;
  postbackUrl?: string;
  message?: string;
  success: boolean;
  dataCriacao: Date;
  dataAtualizacao?: Date;
}
