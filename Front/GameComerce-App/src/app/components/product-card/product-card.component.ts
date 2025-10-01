import { Component, Input } from '@angular/core';
import { Produto } from 'src/app/model/Produto';

@Component({
  selector: 'app-product-card',
  templateUrl: './product-card.component.html',
  styleUrls: ['./product-card.component.scss']
})
export class ProductCardComponent {

  @Input() produto!: Produto;

  getStarsArray(avaliacao: number): number[] {
    return Array(Math.floor(avaliacao)).fill(0);
  }

  getEmptyStarsArray(avaliacao: number): number[] {
    return Array(5 - Math.floor(avaliacao)).fill(0);
  }

  adicionarAoCarrinho(): void {
    if (this.produto.ativo) {
      console.log('Produto adicionado ao carrinho:', this.produto.nome);
      // Implementar lógica do carrinho depois
    }
  }

  compraRapida(): void {
    if (this.produto.ativo) {
      console.log('Compra rápida:', this.produto.nome);
      // Implementar compra rápida depois
    }
  }

}
