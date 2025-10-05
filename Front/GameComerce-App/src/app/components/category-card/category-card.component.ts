import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';
import { Categoria } from 'src/app/model/Categoria';

@Component({
  selector: 'app-category-card',
  templateUrl: './category-card.component.html',
  styleUrls: ['./category-card.component.scss']
})
export class CategoryCardComponent {

  @Input() categoria!: Categoria;
  @Input() tamanho: 'pequeno' | 'medio' | 'grande' = 'medio';

  constructor(private router: Router) {}

  navegarParaCategoria(): void {
    this.router.navigate(['/produtos'], {
      queryParams: { 
        categoria: this.categoria.slug 
      }
    });
  }

}
