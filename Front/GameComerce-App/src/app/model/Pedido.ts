import { ItemPedido } from './ItemPedido';

export interface Pedido {
  nome?: string;
  cpf?: string;
  email?: string;
  telefone?: string;
  subtotal: number;
  total: number;
  frete: number;
  meioPagamento: string,
  itens: ItemPedido[];
  cupomId?: Number;
  descontoAplicado?: number;
}
