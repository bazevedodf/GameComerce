import { Injectable } from '@angular/core';
import { Categoria } from '../model/Categoria';

@Injectable({
  providedIn: 'root'
})
export class CategoriaService {

  private categorias: Categoria[] = [
    { 
      name: "FORTNITE", 
      hasDropdown: true,
      slug: "fortnite",
      imagem: "https://images.unsplash.com/photo-1550745165-9bc0b252726f?ixlib=rb-4.0.3&auto=format&fit=crop&w=600&q=80",
      subcategories: [
        { name: "CONTAS FORTNITE", slug: "contas-fortnite" },
        { name: "BUNDLES FORTNITE", slug: "bundles-fortnite" },
      ]
    },
    { 
      name: "FREE FIRE", 
      hasDropdown: true,
      slug: "free-fire",
      imagem: "https://images.unsplash.com/photo-1579373903780-fd7d3adf541b?ixlib=rb-4.0.3&auto=format&fit=crop&w=600&q=80",
      subcategories: [
        { name: "CONTAS FREEFIRE", slug: "contas-freefire" },
      ]
    },
    { 
      name: "VALORANT", 
      hasDropdown: false,
      slug: "valorant",
      imagem: "https://images.unsplash.com/photo-1620336655055-bd87b4274d6f?ixlib=rb-4.0.3&auto=format&fit=crop&w=600&q=80"
    },
    { 
      name: "LEAGUE OF LEGENDS", 
      hasDropdown: false,
      slug: "league-of-legends",
      imagem: "https://images.unsplash.com/photo-1534423861386-85a16f5d13fd?ixlib=rb-4.0.3&auto=format&fit=crop&w=600&q=80"
    },
    { 
      name: "ROBLOX", 
      hasDropdown: false,
      slug: "roblox",
      imagem: "https://images.unsplash.com/photo-1651333963431-602a570d43b7?ixlib=rb-4.0.3&auto=format&fit=crop&w=600&q=80"
    },
    { 
      name: "BRAWL STARS", 
      hasDropdown: false,
      slug: "brawl-stars",
      imagem: "https://images.unsplash.com/photo-1542751371-adc38448a05e?ixlib=rb-4.0.3&auto=format&fit=crop&w=600&q=80"
    },
  ];

  constructor() { }

  // Buscar todas as categorias
  getCategorias(): Categoria[] {
    return this.categorias;
  }

  // Buscar categorias com dropdown (populares)
  getCategoriasComDropdown(): Categoria[] {
    return this.categorias.filter(categoria => categoria.hasDropdown);
  }

  // Buscar categoria por slug
  getCategoriaPorSlug(slug: string): Categoria | undefined {
    return this.categorias.find(categoria => categoria.slug === slug);
  }

  // Buscar categoria por nome
  getCategoriaPorNome(name: string): Categoria | undefined {
    return this.categorias.find(categoria => 
      categoria.name.toLowerCase() === name.toLowerCase()
    );
  }

  // Buscar categorias por termo de busca
  buscarCategorias(termo: string): Categoria[] {
    return this.categorias.filter(categoria =>
      categoria.name.toLowerCase().includes(termo.toLowerCase()) ||
      categoria.slug.toLowerCase().includes(termo.toLowerCase())
    );
  }

  // Buscar categorias em destaque (com imagem)
  getCategoriasDestaque(): Categoria[] {
    return this.categorias.filter(categoria => categoria.imagem);
  }

  // Buscar subcategorias de uma categoria
  getSubcategorias(slug: string): any[] {
    const categoria = this.getCategoriaPorSlug(slug);
    return categoria?.subcategories || [];
  }
}