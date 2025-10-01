import { Component, OnInit } from '@angular/core';
import { Categoria } from 'src/app/model/Categoria';
import { Produto } from 'src/app/model/Produto';
import { CategoriaService } from 'src/app/services/categoria.service';
import { ProdutoService } from 'src/app/services/Produto.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {

  produtosDestaque: Produto[] = [];
  categoriasDestaque: Categoria[] = [];

  constructor(
    private produtoService: ProdutoService,
    private categoriaService: CategoriaService
  ) { }

  ngOnInit(): void {
    this.carregarDados();
  }

  carregarDados(): void {
    this.produtosDestaque = this.produtoService.getProdutosDestaque();
    this.categoriasDestaque = this.categoriaService.getCategoriasDestaque();
  }

  onBuscaRealizada(resultados: any): void {
    console.log('Busca da home:', resultados);
    // Aqui podemos navegar para página de resultados
    if (resultados.termo || resultados.categoria) {
      // this.router.navigate(['/produtos'], { queryParams: { busca: resultados.termo, categoria: resultados.categoria } });
    }
  }

}
