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
  categoriaSelecionada: string = 'mais-vendidos';
  termoBusca: string = '';
  ordenacao: string = 'relevancia';
  
  // Paginação
  paginaAtual: number = 1;
  itensPorPagina: number = 12;
  totalItens: number = 0;
  
  // Estados
  isLoading: boolean = false;

  categoriaMaisVendidos: Categoria = {
    id: 0,
    name: "MAIS VENDIDOS",
    slug: "mais-vendidos", 
    descricao: "Os produtos mais populares da loja",
    imagem: "",
    icon: "",
    hasDropdown: false,
  };

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
    this.isLoading = false;

    let produtosCarregados = false;
    let categoriasCarregadas = false;

    const verificarSePodeAplicarFiltros = () => {
      if (produtosCarregados && categoriasCarregadas) {
        this.aplicarFiltros(); //Garante que filtros são aplicados
        this.isLoading = false;
      }
    };

    // VERIFICA SE PRECISA CARREGAR PRODUTOS ESPECÍFICOS OU TODOS
    if (this.categoriaSelecionada === 'mais-vendidos') {
      // Carrega produtos mais vendidos
      this.produtoService.getProdutosDestaque().subscribe({
        next: (produtos) => {
          this.produtos = produtos;
          produtosCarregados = true;
          verificarSePodeAplicarFiltros();
        },
        error: (error) => {
          console.error('Erro ao carregar produtos mais vendidos:', error);
          produtosCarregados = true;
          verificarSePodeAplicarFiltros();
        }
      });
    } else if (this.categoriaSelecionada) {
      // Carrega produtos por categoria
      this.produtoService.getProdutosPorCategoria(this.categoriaSelecionada).subscribe({
        next: (produtos) => {
          this.produtos = produtos;
          produtosCarregados = true;
          verificarSePodeAplicarFiltros();
        },
        error: (error) => {
          console.error('Erro ao carregar produtos por categoria:', error);
          produtosCarregados = true;
          verificarSePodeAplicarFiltros();
        }
      });
    } else if (this.termoBusca) {
      // Carrega produtos por busca
      this.produtoService.buscarProdutos(this.termoBusca).subscribe({
        next: (produtos) => {
          this.produtos = produtos;
          produtosCarregados = true;
          verificarSePodeAplicarFiltros();
        },
        error: (error) => {
          console.error('Erro ao buscar produtos:', error);
          produtosCarregados = true;
          verificarSePodeAplicarFiltros();
        }
      });
    } else {
      // Carrega todos os produtos
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
    }
    
    // Carrega categorias (sempre necessário para filtros)
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
      const novaCategoria = params['categoria'] || '';
      const novoTermoBusca = params['busca'] || '';
      
      //SEMPRE recarrega quando os parâmetros principais mudam
      if (novaCategoria !== this.categoriaSelecionada || novoTermoBusca !== this.termoBusca) {
        this.categoriaSelecionada = novaCategoria;
        this.termoBusca = novoTermoBusca;
        this.ordenacao = params['ordenacao'] || 'relevancia';
        this.carregarDados();
      } else {
        //Apenas atualiza ordenação se não mudou categoria/busca
        this.ordenacao = params['ordenacao'] || 'relevancia';
        this.aplicarFiltros();
      }
    });
  }

  // FILTROS E ORDENAÇÃO (mantido igual - agora funciona com dados reais)
  aplicarFiltros(): void {
    let produtosFiltrados = [...this.produtos];

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

  // MÉTODO PARA RECARREGAR PRODUTOS (útil para quando dados mudam)
  recarregarProdutos(): void {
    this.produtoService.recarregarProdutos();
    this.carregarDados();
  }

  get textoCarregamento(): string {
    if (this.termoBusca) {
      return `Buscando por "${this.termoBusca}"...`;
    }
    if (this.categoriaSelecionada === 'mais-vendidos') {
      return 'Carregando os produtos mais vendidos...';
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
    console.log(`${produto.nome} adicionado ao carrinho!`);
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
    this.categoriaSelecionada = 'mais-vendidos';
    this.termoBusca = '';
    this.ordenacao = 'relevancia';

    this.router.navigate([], {
      relativeTo: this.route,
      queryParams: {}, 
      queryParamsHandling: ''
    });

    this.aplicarFiltros();
  }

  atualizarURL(): void {
    const queryParams: any = {};
    if (this.categoriaSelecionada) queryParams.categoria = this.categoriaSelecionada;
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
    
    //Se mudou a categoria ou busca, RECARREGA OS DADOS
    if (this.categoriaSelecionada || this.termoBusca) {
      this.carregarDados(); // Recarrega produtos específicos da categoria/busca
    } else {
      //Se não tem categoria nem busca, apenas aplica filtros nos produtos já carregados
      this.aplicarFiltros();
    }
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
    if (this.categoriaSelecionada === 'mais-vendidos') {
      return '🔥 Mais Vendidos';
    }
    if (this.categoriaSelecionada) {
      const categoria = this.categorias.find(cat => cat.slug === this.categoriaSelecionada);
      return `🎮 Produtos ${categoria?.name || 'Categoria'}`;
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
