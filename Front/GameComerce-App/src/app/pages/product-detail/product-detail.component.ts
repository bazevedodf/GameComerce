import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Produto } from 'src/app/model/Produto';
import { CartService } from 'src/app/services/cart.service';
import { ProdutoService } from 'src/app/services/Produto.service';

@Component({
  selector: 'app-product-detail',
  templateUrl: './product-detail.component.html',
  styleUrls: ['./product-detail.component.scss']
})
export class ProductDetailComponent {

  produto?: Produto;
  isLoading = false;
  quantidade = 1;
  produtosRelacionados: Produto[] = [];
  currentSlide = 0;

  constructor(
    private route: ActivatedRoute,
    private produtoService: ProdutoService,
    private cartService: CartService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.carregarProduto();
  }

  carregarProduto(): void {
    this.isLoading = true;
    const id = Number(this.route.snapshot.paramMap.get('id'));
    
    if (id) {
      this.produtoService.getProdutoPorId(id).subscribe({
        next: (produto) => {
          this.produto = produto;
          this.carregarProdutosRelacionados();
          this.isLoading = false;
        },
        error: (error) => {
          console.error('Erro ao carregar produto:', error);
          this.isLoading = false;
          // Produto não encontrado ou erro
          this.produto = undefined;
        }
      });
    } else {
      this.isLoading = false;
      this.produto = undefined;
    }
  }

  carregarProdutosRelacionados(): void {
    if (this.produto) {
      this.produtoService.getProdutosPorCategoria(this.produto.categoria.name).subscribe({
      next: (produtos) => {
        this.produtosRelacionados = produtos
          .filter(p => p.id !== this.produto!.id)
          .slice(0, 4);
      },
      error: (error) => {
        console.error('Erro ao carregar produtos relacionados:', error);
        this.produtosRelacionados = [];
      }
    });
    }
  }

  comprarAgora(): void {
    if (this.produto) {
      for (let i = 0; i < this.quantidade; i++) {
        this.cartService.adicionarItem(this.produto);
      }
      
      //Navegar para shopping-cart page
      this.router.navigate(['/carrinho']);
    }
  }

  adicionarAoCarrinho(): void {
    if (this.produto) {
      for (let i = 0; i < this.quantidade; i++) {
        this.cartService.adicionarItem(this.produto);
      }
    }

    // ABRIR O CART-SIDEBAR
    this.cartService.openSidebar();
  }

  alterarQuantidade(valor: number): void {
    this.quantidade = Math.max(1, this.quantidade + valor);
  }

  getStarsArray(avaliacao: number): number[] {
    return Array(Math.floor(avaliacao)).fill(0);
  }

  getEmptyStarsArray(avaliacao: number): number[] {
    return Array(5 - Math.floor(avaliacao)).fill(0);
  }

  nextSlide(): void {
    if (this.currentSlide < this.produtosRelacionados.length - 1) {
      this.currentSlide++;
    }
  }

  prevSlide(): void {
    if (this.currentSlide > 0) {
      this.currentSlide--;
    }
  }

  goToSlide(index: number): void {
    this.currentSlide = index;
  }

}
