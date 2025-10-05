import { Categoria } from "./Categoria";

export interface Produto {
    id: number;
    nome: string;
    descricao: string;
    preco: number;
    precoOriginal: number;
    desconto: number;
    imagem: string;
    categoria: Categoria;
    avaliacao: number;
    totalAvaliacoes: number;
    tags: string[];
    ativo: boolean;
    emDestaque: boolean;
    plataforma: string;
    entrega: string;
}
