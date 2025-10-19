import { ItemPedido } from "./ItemPedido";
import { Cupom } from '../../../../GameComerce-App/src/app/model/Cupom';

export interface Pedido {
  id: number;
  email: string;
  telefone: string;
  total: number;
  frete: number;
  status?: string; // "pending", "paid", "expired"
  descontoAplicado?: number;
  dataCriacao: Date;
  meioPagamento: string; // "Pix", "CartaoCredito", "Boleto"
  cupom?: Cupom;
  siteInfoId?: number;
  itens?: ItemPedido[];
}
