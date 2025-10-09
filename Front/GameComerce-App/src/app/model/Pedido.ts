import { ItemPedido } from './ItemPedido';

export interface Pedido {
  email: string;
  telefone?: string;
  total: number;
  frete: number;
  meioPagamento: string,
  itens: ItemPedido[];
  cupom?: string;
  descontoAplicado?: number;
}