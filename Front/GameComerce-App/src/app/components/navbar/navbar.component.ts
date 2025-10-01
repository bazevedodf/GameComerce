import { Component, Input, OnInit } from '@angular/core';
import { Categoria } from 'src/app/model/Categoria';
import { Subcategoria } from 'src/app/model/Subcategoria';
import { CategoriaService } from 'src/app/services/categoria.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent implements OnInit {

  @Input() siteInfo: any;

  cartItemsCount: number = 3;
  categories: Categoria[] = [];

  constructor(private categoriaService: CategoriaService) { }

  ngOnInit(): void {
    this.categories = this.categoriaService.getCategorias();
  }

  // Método para atualizar o contador do carrinho
  updateCartCount(count: number): void {
    this.cartItemsCount = count;
  }

  onBuscaRealizada(resultados: any): void {
    console.log('Busca realizada:', resultados);
    // Aqui podemos navegar para página de resultados ou mostrar em modal
    if (resultados.termo || resultados.categoria) {
      // Navegar para página de resultados de busca
      // this.router.navigate(['/buscar'], { queryParams: { q: resultados.termo, cat: resultados.categoria } });
    }
  }

  navigateToCategory(category: Categoria): void {
    if (!category.hasDropdown) {
      console.log('Navegando para categoria:', category.name);
      // Implementar navegação real depois
    }
  }

  navigateToSubcategory(subcategory: Subcategoria, category: Categoria): void {
    console.log('Navegando para subcategoria:', category.name, '-', subcategory.name);
    console.log('Slug:', subcategory.slug);
    // Implementar navegação real depois
  }
}
