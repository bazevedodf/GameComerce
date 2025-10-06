import { Component, OnInit, ViewChild } from '@angular/core';
import { Categoria } from 'src/app/model/Categoria';
import { Produto } from 'src/app/model/Produto';
import { CategoriaService } from 'src/app/services/categoria.service';
import { ProdutoService } from 'src/app/services/Produto.service';
import { SiteInfoService } from 'src/app/services/SiteInfo.service';
import { SiteInfo } from 'src/app/model/siteInfo';
import { environment } from '@environments/environment';
import { ToastMessageComponent } from '@app/components/toast-message/toast-message.component';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {

  @ViewChild('toastMessage') toastMessage!: ToastMessageComponent;
  toastType: 'error' | 'success' | 'warning' | 'info' = 'error';
  toastMessageText: string = '';

  isLoading = true; // Começa carregando
  loadingMessage = 'Carregando ofertas incríveis...';
  loadingSubMessage = '';
  showProgressBar = true;
  loadingProgress = 0;
  imgCategoriaUlr = environment.imgUrl;

  produtosDestaque: Produto[] = [];
  categoriasDestaque: Categoria[] = [];
  siteInfo?: SiteInfo;

  constructor(
    private produtoService: ProdutoService,
    private categoriaService: CategoriaService,
    private siteInfoService: SiteInfoService
  ) { }

  ngOnInit(): void {
    this.carregarTodosDados();
  }

  carregarTodosDados(): void {
    this.isLoading = true;
    this.loadingMessage = 'Preparando ofertas incríveis...';
    this.showProgressBar = true;
    this.loadingProgress = 0;

    // Simula progresso inicial
    this.simularProgressoInicial();
    
    // Carrega dados em paralelo
    this.carregarDadosReais();
  }

  simularProgressoInicial(): void {
    const interval = setInterval(() => {
      this.loadingProgress += 5;
      
      if (this.loadingProgress === 25) {
        this.loadingMessage = 'Carregando produtos...';
      } else if (this.loadingProgress === 50) {
        this.loadingMessage = 'Organizando categorias...';
      } else if (this.loadingProgress === 75) {
        this.loadingMessage = 'Quase lá...';
      }
      
      // Para em 80% para aguardar a API
      if (this.loadingProgress >= 80) {
        clearInterval(interval);
      }
    }, 100);
  }

  carregarDadosReais(): void {
    // Carrega SiteInfo primeiro (já tem cache)
    this.siteInfoService.getSiteInfo().subscribe({
      next: (siteInfo) => {
        this.siteInfo = siteInfo;
        this.atualizarProgresso(85);
      },
      error: (error) => {
        console.error('Erro ao carregar site info:', error);
        this.atualizarProgresso(85);
      }
    });

    // Carrega categorias em destaque
    this.categoriaService.getCategoriasDestaque().subscribe({
      next: (categorias: Categoria[]) => {
        this.categoriasDestaque = categorias;
        this.atualizarProgresso(95);
        this.finalizarCarregamento();
      },
      error: (error) => {
        console.error('Erro ao carregar categorias:', error);
        this.mostrarToast('error', 'Erro ao carregar categorias. Tente novamente mais tarde.');
        this.atualizarProgresso(95);
        this.finalizarCarregamento();
      }
    });

    // Carrega produtos em destaque
    this.produtoService.getProdutos().subscribe({
      next: (produtos) => {
        this.produtosDestaque = produtos;
        this.atualizarProgresso(90);
      },
      error: (error) => {
        this.mostrarToast('error', 'Erro ao carregar produtos. Tente novamente mais tarde.');
        console.error('Erro ao carregar produtos:', error);
        this.atualizarProgresso(90);
      }
    });
    
  }

  atualizarProgresso(progresso: number): void {
    this.loadingProgress = progresso;
  }

  private mostrarToast(type: 'error' | 'success' | 'warning' | 'info', message: string): void {
    this.toastType = type;
    this.toastMessageText = message;
    this.toastMessage.show();
  }

  finalizarCarregamento(): void {
    // Pequeno delay para mostrar 100%
    setTimeout(() => {
      this.loadingProgress = 100;
      this.loadingMessage = 'Pronto!';
      
      setTimeout(() => {
        this.isLoading = false;
        this.showProgressBar = false;
      }, 500);
    }, 300);
  }

  getCategoriaImagem(slug: string): string {
    let imagem = '';    
    // Busca nas categorias já carregadas
    const categoria = this.categoriasDestaque.find(cat => cat.slug === slug);
    if (categoria) {
      return categoria.imagem || imagem;
    }
    
    return imagem;
  }
}