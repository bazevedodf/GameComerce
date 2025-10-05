import { Component, ElementRef, EventEmitter, Input, Output } from '@angular/core';
import { Router } from '@angular/router';
import { Produto } from 'src/app/model/Produto';
import { CartService } from 'src/app/services/cart.service';

@Component({
  selector: 'app-product-card',
  templateUrl: './product-card.component.html',
  styleUrls: ['./product-card.component.scss']
})
export class ProductCardComponent {

  @Input() produto!: Produto;
  @Output() adicionadoAoCarrinho = new EventEmitter<Produto>();
  
  reflectionTransform: string = 'translateX(-87%)';
  showAddToCart: boolean = false

  constructor(private elementRef: ElementRef,
              private cartService: CartService,
              private router: Router ) { }

  getStarsArray(avaliacao: number): number[] {
    return Array(Math.floor(avaliacao)).fill(0);
  }

  getEmptyStarsArray(avaliacao: number): number[] {
    return Array(5 - Math.floor(avaliacao)).fill(0);
  }

  formatProductName(nome: string): string {
    // Mantém o formato original do nome do produto
    return nome;
  }

  adicionarAoCarrinho(event: Event): void {
    event.preventDefault(); // Impedir navegação
    event.stopPropagation(); // Impedir bubbling
    
    // Adicionar ao carrinho via serviço
    this.cartService.adicionarItem(this.produto);

    // ABRIR O CART-SIDEBAR
    this.cartService.openSidebar();
    
    // Emitir evento para o componente pai (opcional)
    this.adicionadoAoCarrinho.emit(this.produto);
    
    // Feedback visual (podemos adicionar toast depois)
    this.mostrarFeedbackAdicao();
  }

  comprarAgora(event: Event): void {
    event.preventDefault();
    event.stopPropagation();

    // Adiciona ao carrinho
    this.cartService.adicionarItem(this.produto);
    
    // Fecha o sidebar se estiver aberto
    this.cartService.closeSidebar();
    
    // Navega para a página do carrinho
    this.router.navigate(['/carrinho']);
  }

  // Feedback visual temporário
  private mostrarFeedbackAdicao(): void {
    const btn = this.elementRef.nativeElement.querySelector('.add-to-cart-btn');
    if (btn) {
      const originalText = btn.innerHTML;
      btn.innerHTML = '<i class="bi bi-check-lg me-2"></i>Adicionado!';
      btn.classList.add('btn-success');
      btn.classList.remove('btn-warning');
      
      setTimeout(() => {
        btn.innerHTML = originalText;
        btn.classList.remove('btn-success');
        btn.classList.add('btn-warning');
      }, 2000);
    }
  }

  //Efeitos 3D no hover e Mostrar/ocultar botão no hover
  onMouseEnter(event: MouseEvent): void {
    const card = this.elementRef.nativeElement;
    card.style.transform = 'perspective(1000px) rotateX(5deg) rotateY(5deg)';
    card.style.transition = 'transform 0.5s';
    
    this.showAddToCart = true;
  }

  onMouseLeave(event: MouseEvent): void {
    const card = this.elementRef.nativeElement;
    card.style.transform = 'perspective(1000px) rotateX(0deg) rotateY(0deg)';
    this.reflectionTransform = 'translateX(-87%)';

    this.showAddToCart = false; // Ocultar botão carrinho
  }

  onMouseMove(event: MouseEvent): void {
    const card = this.elementRef.nativeElement;
    const cardRect = card.getBoundingClientRect();
    const centerX = cardRect.left + cardRect.width / 2;
    
    // Efeito de reflection baseado na posição do mouse
    const mouseX = event.clientX - cardRect.left;
    const reflectionPosition = (mouseX / cardRect.width) * 100 - 87;
    this.reflectionTransform = `translateX(${reflectionPosition}%)`;
    
    // Efeito 3D suave
    const rotateY = ((event.clientX - centerX) / cardRect.width) * 5;
    const rotateX = ((cardRect.height / 2 - (event.clientY - cardRect.top)) / cardRect.height) * 5;
    
    card.style.transform = `perspective(1000px) rotateX(${rotateX}deg) rotateY(${rotateY}deg)`;
  }

}
