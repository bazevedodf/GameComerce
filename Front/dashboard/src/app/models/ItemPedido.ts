import { Produto } from "./Produto";

export interface ItemPedido {
  id: number;
  pedidoId?: number;
  produtoId: number;
  produto?: Produto;
  quantidade: number;
  precoUnitario: number;
  subtotal: number;
}
