import { Injectable } from '@angular/core';
import { Observable, of, tap, shareReplay, catchError, map } from 'rxjs';
import { Categoria } from '../model/Categoria';
import { Subcategoria } from '../model/Subcategoria';
import { HttpClient } from '@angular/common/http';
import { environment } from '@environments/environment';

@Injectable({
  providedIn: 'root'
})
export class CategoriaService {

  private categoriasCache$?: Observable<Categoria[]>;
  private apiUrl = environment.apiUrl+'Loja';
  private imgUrl = environment.imgUrl;

  constructor(private http: HttpClient) {}

  //MÉTODO PRINCIPAL - Busca categorias UMA VEZ e compartilha
  private carregarCategorias(): Observable<Categoria[]> {
    if (!this.categoriasCache$) {
      this.categoriasCache$ = this.http.get<Categoria[]>(`${this.apiUrl}/categorias`).pipe(
        map(categorias => this.prefixarUrls(categorias)), //Prefixa URLs aqui
        shareReplay(1),
        catchError(error => {
          this.categoriasCache$ = undefined;
          throw error;
        })
      );
    }
    return this.categoriasCache$;
  }

  //MÉTODO PARA PREFIXAR URLs NAS IMAGENS E ÍCONES
  private prefixarUrls(categorias: Categoria[]): Categoria[] {
    return categorias.map(categoria => ({
      ...categoria,
      imagem: categoria.imagem ? this.prefixarUrl(categoria.imagem) : categoria.imagem,
      icon: categoria.icon ? this.prefixarUrl(categoria.icon) : categoria.icon,
    }));
  }

  //MÉTODO PARA PREFIXAR URL COMPLETA
  private prefixarUrl(url: string): string {
    if (url.startsWith('http')) {
      return url;
    }
    const caminho = url.startsWith('/') ? url.substring(1) : url;

    return `${this.imgUrl}/${caminho}`;
  }

  //Buscar todas as categorias (usa cache compartilhado)
  getCategorias(): Observable<Categoria[]> {
    return this.carregarCategorias();
  }

  //Buscar categorias com dropdown (filtra do cache)
  getCategoriasComDropdown(): Observable<Categoria[]> {
    return this.carregarCategorias().pipe(
      tap(categorias => {
        console.log('Filtrando categorias com dropdown');
      }),
      map(categorias => categorias.filter(cat => cat.hasDropdown))
    );
  }

  //Buscar categorias em destaque (filtra do cache)
  getCategoriasDestaque(): Observable<Categoria[]> {
    return this.carregarCategorias().pipe(
      map(categorias => categorias.filter(cat => cat.imagem))
    );
  }

  //Buscar categoria por slug (filtra do cache)
  getCategoriaPorSlug(slug: string): Observable<Categoria | undefined> {
    return this.carregarCategorias().pipe(
      map(categorias => categorias.find(cat => cat.slug === slug))
    );
  }

  //Buscar categoria por nome (filtra do cache)
  getCategoriaPorNome(name: string): Observable<Categoria | undefined> {
    return this.carregarCategorias().pipe(
      map(categorias => categorias.find(cat => 
        cat.name.toLowerCase() === name.toLowerCase()
      ))
    );
  }

  //Buscar categorias por termo (filtra do cache)
  buscarCategorias(termo: string): Observable<Categoria[]> {
    return this.carregarCategorias().pipe(
      map(categorias => categorias.filter(categoria =>
        categoria.name.toLowerCase().includes(termo.toLowerCase()) ||
        categoria.slug.toLowerCase().includes(termo.toLowerCase())
      ))
    );
  }

  //Buscar subcategorias (filtra do cache)
  getSubcategorias(slug: string): Observable<Subcategoria[]> {
    return this.getCategoriaPorSlug(slug).pipe(
      map(categoria => categoria?.subcategories || [])
    );
  }

  // 🔄 Forçar recarregamento (útil quando dados mudam)
  recarregarCategorias(): void {
    this.categoriasCache$ = undefined;
    console.log('Cache de categorias resetado');
  }

  // 🎯 Verificar se já tem cache
  temCache(): boolean {
    return this.categoriasCache$ !== undefined;
  }
}