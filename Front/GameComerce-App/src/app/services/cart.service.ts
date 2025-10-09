import { Injectable } from '@angular/core';
import { Produto } from '@app/model/Produto';
import { BehaviorSubject } from 'rxjs';
import { CartItem } from '@app/model/CartIem'

@Injectable({
  providedIn: 'root'
})
export class CartService {
  
  private cartItemsSubject = new BehaviorSubject<CartItem[]>([]);
  public cartItems$ = this.cartItemsSubject.asObservable();
  
  private isOpenSubject = new BehaviorSubject<boolean>(false);
  public isOpen$ = this.isOpenSubject.asObservable();

  constructor() {
    this.carregarCarrinho();
  }

  // Gerenciar estado do sidebar
  openSidebar(): void {
    this.isOpenSubject.next(true);
    document.body.style.overflow = 'hidden';
  }

  closeSidebar(): void {
    this.isOpenSubject.next(false);
    document.body.style.overflow = 'auto';
  }

  toggleSidebar(): void {
    const currentState = this.isOpenSubject.value;
    this.isOpenSubject.next(!currentState);
    
    if (!currentState) {
      document.body.style.overflow = 'hidden';
    } else {
      document.body.style.overflow = 'auto';
    }
  }

  // Gerenciar itens do carrinho
  adicionarItem(produto: Produto): void {
    const currentItems = this.cartItemsSubject.value;
    const itemExistente = currentItems.find(item => item.produto.id === produto.id);
    
    let novosItens: CartItem[];
    
    if (itemExistente) {
      novosItens = currentItems.map(item => 
        item.produto.id === produto.id 
          ? { ...item, quantidade: item.quantidade + 1 }
          : item
      );
    } else {
      novosItens = [...currentItems, { produto, quantidade: 1 }];
    }
    
    this.cartItemsSubject.next(novosItens);
    this.salvarCarrinho();
  }

  removerItem(produtoId: number): void {
    const currentItems = this.cartItemsSubject.value;
    const novosItens = currentItems.filter(item => item.produto.id !== produtoId);
    
    this.cartItemsSubject.next(novosItens);
    this.salvarCarrinho();
  }

  alterarQuantidade(produtoId: number, novaQuantidade: number): void {
    if (novaQuantidade <= 0) {
      this.removerItem(produtoId);
      return;
    }

    const currentItems = this.cartItemsSubject.value;
    const novosItens = currentItems.map(item => 
      item.produto.id === produtoId 
        ? { ...item, quantidade: novaQuantidade }
        : item
    );
    
    this.cartItemsSubject.next(novosItens);
    this.salvarCarrinho();
  }

  limparCarrinho(): void {
    this.cartItemsSubject.next([]);
    this.salvarCarrinho();
  }

  // Cálculos
  calcularSubtotal(): number {
    return this.cartItemsSubject.value.reduce((total, item) => {
      return total + (item.produto.preco * item.quantidade);
    }, 0);
  }

  calcularFrete(): number {
    return this.calcularSubtotal() > 100 ? 0 : 9.90;
  }

  calcularTotal(): number {
    return this.calcularSubtotal() + this.calcularFrete();
  }

  getTotalItens(): number {
    return this.cartItemsSubject.value.reduce((total, item) => total + item.quantidade, 0);
  }

  // Persistência
  private salvarCarrinho(): void {
    localStorage.setItem('cartItems', JSON.stringify(this.cartItemsSubject.value));
  }

  private carregarCarrinho(): void {
    const saved = localStorage.getItem('cartItems');
    if (saved) {
      try {
        const items = JSON.parse(saved);
        this.cartItemsSubject.next(items);
      } catch (e) {
        console.error('Erro ao carregar carrinho:', e);
      }
    }
  }
}
