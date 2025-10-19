import { Categoria } from "./Categoria";
import { SiteInfo } from "./SiteInfo";

export interface Produto {
  id: number;
  nome: string;
  descricao?: string;
  preco: number;
  precoOriginal: number;
  desconto: number;
  imagem?: string;
  avaliacao?: number;
  totalAvaliacoes: number;
  tags?: string[];
  ativo: boolean;
  emDestaque: boolean;
  entrega: string;
  dataCadastro: Date;
  dataAtualizacao?: Date;
  categoriaId: number;
  categoria?: Categoria;
  siteInfoId: number;
  siteInfo?: SiteInfo;
}
