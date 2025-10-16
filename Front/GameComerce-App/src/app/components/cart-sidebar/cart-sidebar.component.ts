import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CartItem } from '@app/model/CartIem';
import { Produto } from 'src/app/model/Produto';
import { CartService } from 'src/app/services/cart.service';

@Component({
  selector: 'app-cart-sidebar',
  templateUrl: './cart-sidebar.component.html',
  styleUrls: ['./cart-sidebar.component.scss']
})
export class CartSidebarComponent implements OnInit {

  isOpen = false;
  cartItems: CartItem[] = [];
  subtotal = 0;
  frete = 0;
  total = 0;

  constructor(private cartService: CartService, private router: Router) { }

  ngOnInit(): void {
    // Observar estado do sidebar
    this.cartService.isOpen$.subscribe(isOpen => {
      this.isOpen = isOpen;
    });

    // Observar itens do carrinho
    this.cartService.cartItems$.subscribe(items => {
      this.cartItems = items;
      this.calcularTotais();
    })
  }

  // Métodos simplificados - agora delegam para o serviço
  closeSidebar(): void {
    this.cartService.closeSidebar();
  }

  alterarQuantidade(produtoId: number, novaQuantidade: number): void {
    this.cartService.alterarQuantidade(produtoId, novaQuantidade);
  }

  removerItem(produtoId: number): void {
    this.cartService.removerItem(produtoId);
  }

  limparCarrinho(): void {
    this.cartService.limparCarrinho();
  }

  // Cálculos locais
  calcularTotais(): void {
    this.subtotal = this.cartService.calcularSubtotal();
    //this.frete = this.cartService.calcularFrete();
    this.total = this.cartService.calcularTotal();
  }

  getTotalItens(): number {
    return this.cartService.getTotalItens();
  }

  // Navegação
  irParaCheckout(): void {
    this.closeSidebar();
    this.router.navigate(['/carrinho']);
  }
  

  continuarComprando(): void {
    this.closeSidebar();
  }
  
}
