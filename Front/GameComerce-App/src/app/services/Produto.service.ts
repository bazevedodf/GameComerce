import { Injectable } from '@angular/core';
import { Produto } from '../model/Produto';
import { HttpClient } from '@angular/common/http';
import { catchError, map, Observable, of, shareReplay, tap } from 'rxjs';
import { environment } from '@environments/environment';

@Injectable({
  providedIn: 'root',
})
export class ProdutoService {
  
  private useMock = false; // Mude para false quando API estiver pronta
  private produtosCache$?: Observable<Produto[]>;
  private produtosDestaqueCache$?: Observable<Produto[]>;
  private apiUrl = environment.apiUrl+'Produtos';
  private imgUrl = environment.imgUrl;

  constructor(private http: HttpClient) {

  }

  private carregarProdutosIniciais(): void {
    
  }

  // MÉTODO PRINCIPAL - Busca produtos UMA VEZ e compartilha (igual CategoriaService)
  private carregarProdutos(): Observable<Produto[]> {
    if (!this.produtosCache$) {
      this.produtosCache$ = this.http.get<Produto[]>(this.apiUrl).pipe(
        map(produtos => this.prefixarUrlsProdutos(produtos)), // ← AQUI ESTÁ O SEGREDO!
        shareReplay(1),
        catchError(error => {
          this.produtosCache$ = undefined;
          throw error;
        })
      );
    }
    return this.produtosCache$;
  }

  // MÉTODO PARA PREFIXAR URLs NAS IMAGENS DOS PRODUTOS
  private prefixarUrlsProdutos(produtos: Produto[]): Produto[] {
    return produtos.map(produto => ({
      ...produto,
      imagem: produto.imagem ? this.prefixarUrl(produto.imagem) : produto.imagem,
      // Se a categoria também precisa de URLs prefixadas:
      categoria: produto.categoria ? {
        ...produto.categoria,
        imagem: produto.categoria.imagem ? this.prefixarUrl(produto.categoria.imagem) : produto.categoria.imagem,
        icon: produto.categoria.icon ? this.prefixarUrl(produto.categoria.icon) : produto.categoria.icon,
      } : produto.categoria
    }));
  }

  // MÉTODO PARA PREFIXAR URL COMPLETA (igual CategoriaService)
  private prefixarUrl(url: string): string {
    if (url.startsWith('http')) {
      return url;
    }
    const caminho = url.startsWith('/') ? url.substring(1) : url;
    return `${this.imgUrl}/${caminho}`;
  }

  // MÉTODO PARA BUSCAR TODOS OS PRODUTOS
  getProdutos(): Observable<Produto[]> {
    return this.carregarProdutos();
  }

  // MÉTODO PARA BUSCAR PRODUTOS EM DESTAQUE (CORRIGIDO)
  getProdutosDestaque(): Observable<Produto[]> {
    if (!this.produtosDestaqueCache$) {
      this.produtosDestaqueCache$ = this.http.get<Produto[]>(`${this.apiUrl}/destaques`).pipe(
        map(produtos => this.prefixarUrlsProdutos(produtos)),
        shareReplay(1),
        catchError(error => {
          this.produtosDestaqueCache$ = undefined;
          throw error;
        })
      );
    }
    return this.produtosDestaqueCache$;
  }

  // MÉTODO PARA BUSCAR PRODUTO POR ID (CORRIGIDO)
  getProdutoPorId(id: number): Observable<Produto | undefined> {
    return this.http.get<Produto>(`${this.apiUrl}/${id}`).pipe(
      map(produto => this.prefixarUrlsProdutos([produto])[0])
    );
  }

  // MÉTODO PARA BUSCAR PRODUTOS POR CATEGORIA (COM TRATAMENTO DE ERRO MELHORADO)
getProdutosPorCategoria(categoriaSlug: string): Observable<Produto[]> {
  return this.http.get<Produto[]>(`${this.apiUrl}/categoria/${categoriaSlug}`).pipe(
    map(produtos => {
      if (!produtos) {
        console.warn(`Nenhum produto encontrado para categoria: ${categoriaSlug}`);
        return [];
      }
      return this.prefixarUrlsProdutos(produtos);
    }),
    catchError(error => {
      console.error(`Erro ao buscar produtos por categoria ${categoriaSlug}:`, error);
      return of([]); // Retorna array vazio em caso de erro
    })
  );
}

// MÉTODO PARA BUSCAR PRODUTOS POR TERMO DE BUSCA (COM TRATAMENTO DE ERRO MELHORADO)
buscarProdutos(termo: string): Observable<Produto[]> {
  return this.http.get<Produto[]>(`${this.apiUrl}/busca?termo=${termo}`).pipe(
    map(produtos => {
      if (!produtos) {
        console.warn(`Nenhum produto encontrado para busca: ${termo}`);
        return [];
      }
      return this.prefixarUrlsProdutos(produtos);
    }),
    catchError(error => {
      console.error(`Erro ao buscar produtos por termo ${termo}:`, error);
      return of([]); // Retorna array vazio em caso de erro
    })
  );
}

  // MÉTODO PARA RECARREGAR CACHE
  recarregarProdutos(): void {
    this.produtosCache$ = undefined;
    this.produtosDestaqueCache$ = undefined;
    console.log('Cache de produtos resetado');
  }

}
