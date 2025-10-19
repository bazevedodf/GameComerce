export interface Categoria {
  id: number;
  name: string;
  slug: string;
  descricao?: string;
  imagem?: string;
  icon?: string;
  ativo: boolean;
  siteInfoId: number;
}
