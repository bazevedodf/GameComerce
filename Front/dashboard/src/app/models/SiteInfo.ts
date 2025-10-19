export interface SiteInfo {
  id: number;
  nome: string;
  dominio: string;
  logoUrl?: string;
  cnpj: string;
  address: string;
  email: string;
  instagram: string;
  facebook: string;
  whatsapp: string;
  apiKey?: string;
  baseUrl?: string;
  ativo: boolean;

  cupons?: any[];
  pedidos?: any[];
  produtos?: any[];
  categorias?: any[];
  marketingTags?: any[];
}
