import { Produto } from './Produto';

export interface Pedido {
  email: string;
  telefone?: string;
  total: number;
  frete: number;
  meioPagamento: string,
  itens: Produto[];
  cupom?: string;
  descontoAplicado?: number;
}