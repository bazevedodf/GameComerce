import { MarketingTag } from "./MarketingTag";

export interface SiteInfo {
  nome: string;
  dominio?: string;
  logoUrl?: string;
  cnpj?: string;
  address?: string;
  email?: string;
  instagram?: string;
  facebook?: string;
  whatsapp?: string;
  youTube?: string;
  marketingTags?: MarketingTag[];
}