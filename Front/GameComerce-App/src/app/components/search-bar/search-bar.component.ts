import { Component, ElementRef, EventEmitter, OnInit, Output, ViewChild } from '@angular/core';
import { Categoria } from 'src/app/model/Categoria';
import { Produto } from 'src/app/model/Produto';
import { CategoriaService } from 'src/app/services/categoria.service';
import { ProdutoService } from 'src/app/services/Produto.service';

@Component({
  selector: 'app-search-bar',
  templateUrl: './search-bar.component.html',
  styleUrls: ['./search-bar.component.scss']
})
export class SearchBarComponent implements OnInit {

  @Output() buscaRealizada = new EventEmitter<any>();
  @ViewChild('searchInput') searchInput!: ElementRef;

  termoBusca: string = '';
  categoriaFiltro: string = '';
  mostrarSugestoes: boolean = false;
  mostrarModalMobile: boolean = false;
  
  categorias: Categoria[] = [];
  sugestoes: any[] = [];

  constructor(
    private categoriaService: CategoriaService,
    private produtoService: ProdutoService
  ) { }

  ngOnInit(): void {
    this.carregarCategorias();
  }

  carregarCategorias(): void {
    this.categorias = this.categoriaService.getCategorias();
  }

  onBuscar(): void {
    if (this.termoBusca.length >= 2) {
      this.buscarSugestoes();
    } else {
      this.sugestoes = [];
    }
  }

  onFocus(): void {
    this.mostrarSugestoes = true;
    if (this.termoBusca.length >= 2) {
      this.buscarSugestoes();
    }
  }

  onBlur(): void {
    // Pequeno delay para permitir clique nas sugestões
    setTimeout(() => {
      this.mostrarSugestoes = false;
    }, 200);
  }

  buscarSugestoes(): void {
    const produtos = this.produtoService.getProdutos();
    
    this.sugestoes = produtos
      .filter(produto => 
        produto.nome.toLowerCase().includes(this.termoBusca.toLowerCase()) ||
        produto.descricao.toLowerCase().includes(this.termoBusca.toLowerCase()) ||
        produto.tags.some(tag => tag.toLowerCase().includes(this.termoBusca.toLowerCase()))
      )
      .slice(0, 5) // Limitar a 5 sugestões
      .map(produto => ({
        id: produto.id,
        nome: produto.nome,
        categoria: produto.categoria,
        plataforma: produto.plataforma,
        preco: produto.preco,
        produto: produto
      }));
  }

  selecionarSugestao(sugestao: any): void {
    this.termoBusca = sugestao.nome;
    this.mostrarSugestoes = false;
    this.realizarBusca();
  }

  aplicarFiltroCategoria(slug: string): void {
    this.categoriaFiltro = slug;
    this.realizarBusca();
  }

  realizarBusca(): void {
    const resultados = {
      termo: this.termoBusca,
      categoria: this.categoriaFiltro,
      produtos: this.obterResultadosBusca()
    };

    this.buscaRealizada.emit(resultados);
    
    // Focar no input novamente após busca
    this.searchInput.nativeElement.focus();
  }

  obterResultadosBusca(): Produto[] {
    let produtos = this.produtoService.getProdutos();

    // Aplicar filtro de termo
    if (this.termoBusca) {
      produtos = produtos.filter(produto =>
        produto.nome.toLowerCase().includes(this.termoBusca.toLowerCase()) ||
        produto.descricao.toLowerCase().includes(this.termoBusca.toLowerCase()) ||
        produto.tags.some(tag => tag.toLowerCase().includes(this.termoBusca.toLowerCase()))
      );
    }

    // Aplicar filtro de categoria
    if (this.categoriaFiltro) {
      const categoria = this.categoriaService.getCategoriaPorSlug(this.categoriaFiltro);
      if (categoria) {
        produtos = produtos.filter(produto => 
          produto.plataforma.toLowerCase() === categoria.name.toLowerCase()
        );
      }
    }

    return produtos;
  }

  limparBusca(): void {
    this.termoBusca = '';
    this.categoriaFiltro = '';
    this.sugestoes = [];
    this.buscaRealizada.emit({ termo: '', categoria: '', produtos: [] });
  }

}
