import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { Categoria } from '../model/Categoria';
import { Subcategoria } from '../model/Subcategoria';

@Injectable({
  providedIn: 'root'
})
export class CategoriaService {

  private useMock = true; // Mude para false quando API estiver pronta
  private categoriasCache: Categoria[] = [];
  private categoriasDestaqueCache: Categoria[] = [];
  private categoriasComDropdownCache: Categoria[] = [];
  private apiUrl = 'https://sua-api.com/api';

  constructor() {
    this.carregarCategoriasIniciais();
  }

  private carregarCategoriasIniciais(): void {
    if (this.useMock) {
      this.categoriasCache = [
        { 
          id: 1,
          name: "FORTNITE", 
          hasDropdown: true,
          slug: "fortnite",
          descricao: "Explore, crie e brilhe no Roblox com estilo!",
          imagem: "../../assets/img/categoria-fortnite.png",
          icon: "assets/img/fortnite-ico.png",
          subcategories: [
            { name: "CONTAS FORTNITE", slug: "contas-fortnite" },
            { name: "BUNDLES FORTNITE", slug: "bundles-fortnite" },
          ]
        },
        { 
          id: 2,
          name: "FREE FIRE", 
          hasDropdown: true,
          slug: "free-fire",
          descricao: "Ação rápida e intensa em batalhas épicas.",
          imagem: "../../assets/img/categoria-free-fire.webp",
          icon: "assets/img/freefire-ico.png",
          subcategories: [
            { name: "CONTAS FREEFIRE", slug: "contas-freefire" },
          ]
        },
        { 
          id: 3,
          name: "VALORANT", 
          hasDropdown: false,
          slug: "valorant",
          descricao: "Ação tática com estilo e precisão.",
          imagem: "../../assets/img/categoria-valorant.jpg",
          icon: "assets/img/valorant-ico.png"
        },
        { 
          id: 4,
          name: "LEAGUE OF LEGENDS", 
          hasDropdown: false,
          slug: "league-of-legends",
          descricao: "Batalhas estratégicas em um mundo de fantasia.",
          imagem: "../../assets/img/categoria-legue-of-legend.png",
          icon: "assets/img/legue-of-legends-ico.png"
        },
        { 
          id: 5,
          name: "ROBLOX", 
          hasDropdown: false,
          slug: "roblox",
          descricao: "Explore, crie e brilhe no Roblox com estilo!",
          imagem: "../../assets/img/categoria-roblox.webp",
          icon: "assets/img/robux-ico.png"
        },
        { 
          id: 6,
          name: "BRAWL STARS", 
          hasDropdown: false,
          slug: "brawl-stars",
          descricao: "Lutas rápidas e divertidas com amigos.",
          imagem: "../../assets/img/categoria-brawl-stars.webp",
          icon: "assets/img/brawstars-ico.png"
        },
      ];

      // Pré-carrega os caches
      this.categoriasDestaqueCache = this.categoriasCache.filter(categoria => categoria.imagem);
      this.categoriasComDropdownCache = this.categoriasCache.filter(categoria => categoria.hasDropdown);
    }
  }

  // Buscar todas as categorias
  getCategorias(): Observable<Categoria[]> {
    if (this.useMock) {
      return of(this.categoriasCache);
    } else {
      // Se já tem cache, retorna do cache
      if (this.categoriasCache.length > 0) {
        return of(this.categoriasCache);
      }
      
      // TODO: Implementar chamada API real
      // return this.http.get<Categoria[]>(`${this.apiUrl}/categorias`).pipe(
      //   tap(categorias => {
      //     this.categoriasCache = categorias;
      //     this.categoriasDestaqueCache = categorias.filter(cat => cat.imagem);
      //     this.categoriasComDropdownCache = categorias.filter(cat => cat.hasDropdown);
      //   })
      // );
      return of(this.categoriasCache); // Fallback temporário
    }
  }

  // Buscar categorias com dropdown (populares)
  getCategoriasComDropdown(): Observable<Categoria[]> {
    if (this.useMock) {
      return of(this.categoriasComDropdownCache);
    } else {
      // Se já tem cache, retorna do cache
      if (this.categoriasComDropdownCache.length > 0) {
        return of(this.categoriasComDropdownCache);
      }
      
      // TODO: Implementar chamada API real
      return of(this.categoriasComDropdownCache);
    }
  }

  // Buscar categorias em destaque (com imagem)
  getCategoriasDestaque(): Observable<Categoria[]> {
    if (this.useMock) {
      return of(this.categoriasDestaqueCache);
    } else {
      // Se já tem cache, retorna do cache
      if (this.categoriasDestaqueCache.length > 0) {
        return of(this.categoriasDestaqueCache);
      }
      
      // TODO: Implementar chamada API real
      // return this.http.get<Categoria[]>(`${this.apiUrl}/categorias/destaque`).pipe(
      //   tap(categorias => {
      //     this.categoriasDestaqueCache = categorias;
      //   })
      // );
      return of(this.categoriasDestaqueCache);
    }
  }

  // Buscar categoria por slug
  getCategoriaPorSlug(slug: string): Observable<Categoria | undefined> {
    if (this.useMock) {
      const categoria = this.categoriasCache.find(cat => cat.slug === slug);
      return of(categoria);
    } else {
      // TODO: Implementar chamada API real
      // return this.http.get<Categoria>(`${this.apiUrl}/categorias/${slug}`);
      return of(this.categoriasCache.find(cat => cat.slug === slug));
    }
  }

  // Buscar categoria por nome
  getCategoriaPorNome(name: string): Observable<Categoria | undefined> {
    if (this.useMock) {
      const categoria = this.categoriasCache.find(cat => 
        cat.name.toLowerCase() === name.toLowerCase()
      );
      return of(categoria);
    } else {
      // TODO: Implementar chamada API real
      return of(this.categoriasCache.find(cat => 
        cat.name.toLowerCase() === name.toLowerCase()
      ));
    }
  }

  // Buscar categorias por termo de busca
  buscarCategorias(termo: string): Observable<Categoria[]> {
    if (this.useMock) {
      const categoriasFiltradas = this.categoriasCache.filter(categoria =>
        categoria.name.toLowerCase().includes(termo.toLowerCase()) ||
        categoria.slug.toLowerCase().includes(termo.toLowerCase())
      );
      return of(categoriasFiltradas);
    } else {
      // TODO: Implementar chamada API real
      // return this.http.get<Categoria[]>(`${this.apiUrl}/categorias/busca?q=${termo}`);
      return of(this.categoriasCache.filter(categoria =>
        categoria.name.toLowerCase().includes(termo.toLowerCase()) ||
        categoria.slug.toLowerCase().includes(termo.toLowerCase())
      ));
    }
  }

  // Buscar subcategorias de uma categoria
  getSubcategorias(slug: string): Observable<Subcategoria[]> {
    if (this.useMock) {
      const categoria = this.categoriasCache.find(cat => cat.slug === slug);
      return of(categoria?.subcategories || []);
    } else {
      // TODO: Implementar chamada API real
      // return this.http.get<Subcategoria[]>(`${this.apiUrl}/categorias/${slug}/subcategorias`);
      const categoria = this.categoriasCache.find(cat => cat.slug === slug);
      return of(categoria?.subcategories || []);
    }
  }

  // Método para atualizar quando API estiver pronta
  setUseMock(usarMock: boolean): void {
    this.useMock = usarMock;
    if (!usarMock) {
      // Limpa caches quando mudar para API real
      this.categoriasCache = [];
      this.categoriasDestaqueCache = [];
      this.categoriasComDropdownCache = [];
    }
  }

  // Método para forçar recarregamento (útil para admin)
  recarregarCategorias(): void {
    this.categoriasCache = [];
    this.categoriasDestaqueCache = [];
    this.categoriasComDropdownCache = [];
    this.carregarCategoriasIniciais();
  }
}