import { Subcategoria } from "./Subcategoria";

export interface Categoria {
    name: string;
    hasDropdown: boolean;
    imagem?: string;
    slug: string;
    subcategories?: Subcategoria[];
}
