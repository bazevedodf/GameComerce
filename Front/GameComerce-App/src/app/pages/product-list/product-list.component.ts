import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Categoria } from 'src/app/model/Categoria';
import { Produto } from 'src/app/model/Produto';
import { CartService } from 'src/app/services/cart.service';
import { CategoriaService } from 'src/app/services/categoria.service';
import { ProdutoService } from 'src/app/services/Produto.service';

@Component({
  selector: 'app-product-list',
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.scss']
})
export class ProductListComponent {

  // Listas
  produtos: Produto[] = [];
  produtosFiltrados: Produto[] = [];
  categorias: Categoria[] = [];
  
  // Filtros
  categoriaSelecionada: string = '';
  subcategoriaSelecionada: string = '';
  termoBusca: string = '';
  ordenacao: string = 'relevancia';
  
  // Paginação
  paginaAtual: number = 1;
  itensPorPagina: number = 12;
  totalItens: number = 0;
  
  // Estados
  isLoading: boolean = false;

  constructor(
    private produtoService: ProdutoService,
    private categoriaService: CategoriaService,
    private cartService: CartService,
    private route: ActivatedRoute,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.carregarDados();
    this.observarParametrosRota();
  }
  
  carregarDados(): void {
    this.isLoading = true;

    let produtosCarregados = false;
    let categoriasCarregadas = false;

    const verificarSePodeAplicarFiltros = () => {
      if (produtosCarregados && categoriasCarregadas) {
        this.aplicarFiltros();
        this.isLoading = false;
      }
    };

    
    // Carrega produtos
    this.produtoService.getProdutos().subscribe({
      next: (produtos) => {
        this.produtos = produtos;
        produtosCarregados = true;
        verificarSePodeAplicarFiltros();
      },
      error: (error) => {
        console.error('Erro ao carregar produtos:', error);
        produtosCarregados = true;
        verificarSePodeAplicarFiltros();
      }
    });
    
    // Carrega categorias
    this.categoriaService.getCategorias().subscribe({
      next: (categorias) => {
        this.categorias = categorias;
        categoriasCarregadas = true;
        verificarSePodeAplicarFiltros();
      },
      error: (error) => {
        console.error('Erro ao carregar categorias:', error);
        categoriasCarregadas = true;
        verificarSePodeAplicarFiltros();
      }
    });
  }

  observarParametrosRota(): void {
    this.route.queryParams.subscribe(params => {
      this.categoriaSelecionada = params['categoria'] || '';
      this.subcategoriaSelecionada = params['subcategoria'] || '';
      this.termoBusca = params['busca'] || '';
      this.ordenacao = params['ordenacao'] || 'relevancia';
      
      if (this.produtos.length > 0) {
        this.aplicarFiltros();
      }
    });
  }

  // FILTROS E ORDENAÇÃO
  aplicarFiltros(): void {
    let produtosFiltrados = [...this.produtos];

    // Filtro por categoria
    if (this.categoriaSelecionada) {
      produtosFiltrados = produtosFiltrados.filter(p => 
        p.categoria.slug === this.categoriaSelecionada
      );
    }

    // NOVO: Filtro por subcategoria
    if (this.subcategoriaSelecionada) {
      produtosFiltrados = produtosFiltrados.filter(p => 
        p.categoria.name.toLowerCase() === this.subcategoriaSelecionada.toLowerCase()
      );
    }

    // Filtro por busca
    if (this.termoBusca) {
      const termo = this.termoBusca.toLowerCase();
      produtosFiltrados = produtosFiltrados.filter(p =>
        p.nome.toLowerCase().includes(termo) ||
        p.descricao.toLowerCase().includes(termo) ||
        p.tags.some(tag => tag.toLowerCase().includes(termo))
      );
    }

    // Ordenação
    produtosFiltrados = this.ordenarProdutos(produtosFiltrados);

    this.produtosFiltrados = produtosFiltrados;
    this.totalItens = produtosFiltrados.length;
    this.paginaAtual = 1; // Reset para primeira página
  }

  ordenarProdutos(produtos: Produto[]): Produto[] {
    switch (this.ordenacao) {
      case 'preco-menor':
        return produtos.sort((a, b) => a.preco - b.preco);
      
      case 'preco-maior':
        return produtos.sort((a, b) => b.preco - a.preco);
      
      case 'avaliacao':
        return produtos.sort((a, b) => b.avaliacao - a.avaliacao);
      
      case 'nome':
        return produtos.sort((a, b) => a.nome.localeCompare(b.nome));
      
      case 'relevancia':
      default:
        return produtos.sort((a, b) => {
          // Produtos em destaque primeiro, depois por avaliação
          if (a.emDestaque !== b.emDestaque) {
            return a.emDestaque ? -1 : 1;
          }
          return b.avaliacao - a.avaliacao;
        });
    }
  }

  get textoCarregamento(): string {
    if (this.termoBusca) {
      return `Buscando por "${this.termoBusca}"...`;
    }
    if (this.categoriaSelecionada) {
      const categoria = this.categorias.find(cat => cat.slug === this.categoriaSelecionada);
      return `Carregando ${categoria?.name || 'categoria'}...`;
    }
    return 'Preparando os melhores produtos...';
  }

  // PAGINAÇÃO
  get produtosPaginados(): Produto[] {
    const inicio = (this.paginaAtual - 1) * this.itensPorPagina;
    const fim = inicio + this.itensPorPagina;
    return this.produtosFiltrados.slice(inicio, fim);
  }

  get totalPaginas(): number {
    return Math.ceil(this.totalItens / this.itensPorPagina);
  }

  get paginas(): number[] {
    return Array.from({ length: this.totalPaginas }, (_, i) => i + 1);
  }

  mudarPagina(pagina: number): void {
    if (pagina >= 1 && pagina <= this.totalPaginas) {
      this.paginaAtual = pagina;
      window.scrollTo({ top: 0, behavior: 'smooth' });
    }
  }

  // AÇÕES
  adicionarAoCarrinho(produto: Produto): void {
    this.cartService.adicionarItem(produto);
    
    // Feedback visual (podemos adicionar toast depois)
    console.log(`✅ ${produto.nome} adicionado ao carrinho!`);
  }

  onProdutoAdicionado(produto: Produto): void {
    console.log('Produto adicionado:', produto.nome);
    // Aqui você poderia:
    // - Mostrar um toast notification
    // - Atualizar algum contador extra
    // - Fazer tracking analytics
    // - Mostrar confetti animation
  }

  limparFiltros(): void {
    this.categoriaSelecionada = '';
    this.subcategoriaSelecionada = '';
    this.termoBusca = '';
    this.ordenacao = 'relevancia';
    this.atualizarURL();
    this.aplicarFiltros();
  }

  atualizarURL(): void {
    const queryParams: any = {};
    if (this.categoriaSelecionada) queryParams.categoria = this.categoriaSelecionada;
    if (this.subcategoriaSelecionada) queryParams.subcategoria = this.subcategoriaSelecionada;
    if (this.termoBusca) queryParams.busca = this.termoBusca;
    if (this.ordenacao !== 'relevancia') queryParams.ordenacao = this.ordenacao;

    this.router.navigate([], {
      relativeTo: this.route,
      queryParams,
      queryParamsHandling: 'merge'
    });
  }

  onFiltroChange(): void {
    this.atualizarURL();
    this.aplicarFiltros();
  }

  // GETTERS ÚTEIS PARA TEMPLATE
  get mostrarPaginacao(): boolean {
    return this.totalItens > this.itensPorPagina;
  }

  get mostrarResultados(): boolean {
    return this.produtosFiltrados.length > 0;
  }

  get textoResultados(): string {
    const inicio = (this.paginaAtual - 1) * this.itensPorPagina + 1;
    const fim = Math.min(this.paginaAtual * this.itensPorPagina, this.totalItens);
    
    if (this.totalItens === 0) return 'Nenhum produto encontrado';
    if (this.totalItens === 1) return '1 produto encontrado';
    
    return `${inicio}-${fim} de ${this.totalItens} produtos`;
  }

  get isSearchResults(): boolean {
    return !!this.termoBusca;
  }

  get pageTitle(): string {
    if (this.isSearchResults) {
      return `🔍 Resultados para "${this.termoBusca}"`;
    }
    if (this.categoriaSelecionada) {
      // ✅ CORREÇÃO: Busca nas categorias já carregadas
      const categoria = this.categorias.find(cat => cat.slug === this.categoriaSelecionada);
      return `${categoria?.icon || '🎮'} ${categoria?.name || 'Categoria'}`;
    }
    return '🎮 Todos os Produtos';
  }

  get pageSubtitle(): string {
    if (this.isSearchResults) {
      return `${this.totalItens} produto${this.totalItens !== 1 ? 's' : ''} encontrado${this.totalItens !== 1 ? 's' : ''}`;
    }
    return `${this.totalItens} produto${this.totalItens !== 1 ? 's' : ''} disponível${this.totalItens !== 1 ? 's' : ''}`;
  }

}
