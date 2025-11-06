import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { CartItem } from '@app/model/CartIem';
import { Pedido } from '@app/model/Pedido';
import { CupomService } from '@app/services/cupom.service';
import { MarketingTagService } from '@app/services/marketingTag.service';
import { PedidoService } from '@app/services/pedido.service';
import { Subscription } from 'rxjs';
import { ToastMessageComponent } from 'src/app/components/toast-message/toast-message.component';
import { Cupom } from 'src/app/model/Cupom';
import { CartService } from 'src/app/services/cart.service';

declare var bootstrap: any;

@Component({
  selector: 'app-shopping-cart',
  templateUrl: './shopping-cart.component.html',
  styleUrls: ['./shopping-cart.component.scss']
})
export class ShoppingCartComponent implements OnInit, OnDestroy {

  @ViewChild('toastMessage') toastMessage!: ToastMessageComponent;
  toastType: 'error' | 'success' | 'warning' | 'info' = 'error';
  toastMessageText: string = '';

  cartItems: CartItem[] = [];
  cupom: string = '';
  descontoAplicado: boolean = false;
  valorDesconto: number = 0;
  pedido!: Pedido;

  // Totais
  subtotal: number = 0;
  frete: number = 0;
  total: number = 0;
  totalItens: number = 0;

  // Cupom
  cupomError: string = '';
  isValidandoCupom: boolean = false;
  cupomAplicado?: Cupom;

  constructor(
    private cartService: CartService,
    private pedidoService: PedidoService,
    private cupomService: CupomService,
    private marketingTagService: MarketingTagService,
    private router: Router
  ) {}

  ngOnInit(): void {
    console.log('carregando carrinho');
    this.carregarCarrinho();
  }

  ngOnDestroy(): void {}

  carregarCarrinho(): void {
    this.cartService.cartItems$.subscribe(items => {
      this.cartItems = items;
      this.calcularTotais();
    });
  }

  calcularTotais(): void {
    this.subtotal = this.cartService.calcularSubtotal();

    // Aplicar desconto se houver
    if (this.descontoAplicado) {
      this.total = this.subtotal + this.frete - this.valorDesconto;
    } else {
      this.total = this.subtotal + this.frete;
    }

    this.totalItens = this.cartService.getTotalItens();
  }

  alterarQuantidade(produtoId: number, novaQuantidade: number): void {
    this.cartService.alterarQuantidade(produtoId, novaQuantidade);
  }

  removerItem(produtoId: number): void {
    this.cartService.removerItem(produtoId);
  }

  continuarComprando(): void {
    this.router.navigate(['/produtos']);
  }

  irParaCheckout(): void {
    if (this.cartItems.length === 0) {
      this.mostrarToast('error', 'Seu carrinho está vazio. Adicione produtos antes de ir para o checkout.');
      return;
    }
    this.gerarPerdido();
    // DISPARAR INITIATECHECKOUT
    this.marketingTagService.dispararInitiateCheckout({
      itens: this.cartItems,
      total: this.total,
      subtotal: this.subtotal,
      frete: this.frete
    });
    this.router.navigate(['/checkout']);
  }

  gerarPerdido(): void {
    this.pedido = {
      subtotal: this.subtotal | 0,
      total: this.total | 0,
      frete: this.frete | 0,
      meioPagamento: 'PIX',
      itens: this.cartItems.map(item => ({
        nome: item.produto.nome,
        produtoId: item.produto.id,
        quantidade: item.quantidade,
        precoUnitario: item.produto.preco,
        imagem: item.produto.imagem || 'assets/no-image.svg'
      })),
      cupomId: this.cupomAplicado?.id || undefined,
      descontoAplicado: this.descontoAplicado ? this.valorDesconto : undefined,
    };

    this.pedidoService.adicionarPedido(this.pedido);
  }

  aplicarCupom(): void {
    if (!this.cupom || this.cupom.trim() === '') {
      this.cupomError = 'Digite um código de cupom';
      this.mostrarToast('error', 'Digite um código de cupom');
      return;
    }

    this.isValidandoCupom = true;
    this.cupomError = '';

    this.cupomService.validarCupom(this.cupom.toUpperCase()).subscribe({
      next: (cupom: Cupom) => {
        this.isValidandoCupom = false;

        if (cupom.valido) {
          this.aplicarDesconto(cupom);
          this.mostrarToast('success', `Cupom ${cupom.codigo} aplicado! Desconto de R$ ${this.valorDesconto.toFixed(2)}`);
        } else {
          // Usa a mensagem que já vem do serviço
          this.cupomError = cupom.mensagemErro || 'Cupom inválido';
          this.limparCupom();
          this.mostrarToast('error', this.cupomError);
        }
      },
      error: (error) => {
        this.isValidandoCupom = false;
        this.cupomError = 'Erro ao validar cupom';
        this.limparCupom();
        this.mostrarToast('error', 'Erro de conexão. Tente novamente.');
      }
    });
  }

  private aplicarDesconto(cupom: Cupom): void {
    this.cupomAplicado = cupom;

    if (cupom.tipoDesconto.toLocaleLowerCase() === 'percentual') {
      this.valorDesconto = this.subtotal * (cupom.valorDesconto! / 100);
    } else {
      this.valorDesconto = cupom.valorDesconto!;
    }

    this.descontoAplicado = true;
    this.calcularTotais();
  }

  getPercentualDesconto(): number {
    if (this.cupomAplicado?.tipoDesconto === 'percentual') {
      return this.cupomAplicado.valorDesconto || 0;
    }
    return 0;
  }

  public limparCupom(): void {
    this.cupom = '';
    this.cupomAplicado = undefined;
    this.descontoAplicado = false;
    this.valorDesconto = 0;
    this.calcularTotais();
  }

  private mostrarToast(type: 'error' | 'success' | 'warning' | 'info', message: string): void {
    this.toastType = type;
    this.toastMessageText = message;
    this.toastMessage.show();
  }
}

