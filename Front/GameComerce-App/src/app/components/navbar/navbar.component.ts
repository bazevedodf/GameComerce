import { Component, Input, OnInit } from '@angular/core';
import { Categoria } from 'src/app/model/Categoria';
import { SiteInfo } from 'src/app/model/siteInfo';
import { CartService } from 'src/app/services/cart.service';
import { CategoriaService } from 'src/app/services/categoria.service';
import { SiteInfoService } from 'src/app/services/SiteInfo.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent implements OnInit {

  siteInfo?: SiteInfo;
  cartItemsCount: number = 0;
  categories: Categoria[] = [];
  isMobileMenuOpen = false;

  constructor(private categoriaService: CategoriaService,
              private cartService: CartService,
              private siteInfoService: SiteInfoService) { }

  ngOnInit(): void {
    this.categoriaService.getCategorias().subscribe(categorias => {
      this.categories = categorias;
    });
    
    this.cartService.cartItems$.subscribe(items => {
      this.cartItemsCount = items.reduce((total, item) => total + item.quantidade, 0);
    });
    this.siteInfoService.getSiteInfo().subscribe(data => {
      this.siteInfo = data;
    });
  }

  // Abrir carrinho via serviço
  abrirCarrinho(): void {
    this.cartService.openSidebar();
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

  toggleMobileMenu(): void {
    this.isMobileMenuOpen = !this.isMobileMenuOpen;
    document.body.style.overflow = this.isMobileMenuOpen ? 'hidden' : 'auto';
  }

  closeMobileMenu(): void {
    this.isMobileMenuOpen = false;
    document.body.style.overflow = 'auto';
  }

}
