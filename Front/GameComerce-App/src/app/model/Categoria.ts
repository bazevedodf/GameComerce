import { Subcategoria } from "./Subcategoria";

export interface Categoria {
    id: number;
    name: string;
    hasDropdown: boolean;
    imagem?: string;
    icon?: string;
    slug: string;
    descricao?: string;
    subcategories?: Subcategoria[];
}
