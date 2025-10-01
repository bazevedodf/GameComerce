import { Component, Input } from '@angular/core';
import { Categoria } from 'src/app/model/Categoria';

@Component({
  selector: 'app-category-card',
  templateUrl: './category-card.component.html',
  styleUrls: ['./category-card.component.scss']
})
export class CategoryCardComponent {

  @Input() categoria!: Categoria;
  @Input() tamanho: 'pequeno' | 'medio' | 'grande' = 'medio';

  navegarParaCategoria(): void {
    console.log('Navegando para categoria:', this.categoria.name);
    console.log('Slug:', this.categoria.slug);
    // Implementar navegação para a categoria
  }

}
