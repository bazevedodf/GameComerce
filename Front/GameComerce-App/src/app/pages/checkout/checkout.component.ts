import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastMessageComponent } from '@app/components/toast-message/toast-message.component';
import { CartItem } from '@app/model/CartIem';
import { Cupom } from '@app/model/Cupom';
import { Pedido } from '@app/model/Pedido';
import { PedidoResponse } from '@app/model/PedidoResponse';
import { CartService } from '@app/services/cart.service';
import { MarketingTagService } from '@app/services/marketingTag.service';
import { PedidoService } from '@app/services/pedido.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-checkout',
  templateUrl: './checkout.component.html',
  styleUrls: ['./checkout.component.scss']
})
export class CheckoutComponent implements OnInit, OnDestroy {

  @ViewChild('toastMessage') toastMessage!: ToastMessageComponent;
  toastType: 'error' | 'success' | 'warning' | 'info' = 'error';
  toastMessageText: string = '';

  checkoutForm!: FormGroup;
  cartItems: CartItem[] = [];
  nome: string = '';
  email: string = '';
  telefone: string = '';
  //cpf: string = '';
  cupom: string = '';
  descontoAplicado: boolean = false;
  valorDesconto: number = 0;
  pedido!: Pedido;
  currentStep: number = 1;

  // Totais
  subtotal: number = 0;
  frete: number = 0;
  total: number = 0;
  totalItens: number = 0;

  // Novas variáveis para PIX
  pedidoResponse?: PedidoResponse;
  paymentPollingSubscription?: Subscription;
  tempoRestante: number = 0;
  tempoTotal: number = 0;
  progresso: number = 100;
  private intervalId?: any;

  // Cupom
  cupomError: string = '';
  isValidandoCupom: boolean = false;
  cupomAplicado?: Cupom;

  // Loading
  isLoading: boolean = false;
  loadingMessage: string = 'Processando pagamento...';
  loadingSubMessage: string = 'Gerando código PIX';

   get f(): any {
    return this.checkoutForm.controls;
  }

  constructor(private fb: FormBuilder,
              private pedidoService: PedidoService,
              private cartService: CartService,
              private marketingTagService: MarketingTagService,
              private router: Router) {
                this.carregarPedido();

  }

  ngOnInit(): void {
    this.initializeForm();
  }

  ngOnDestroy(): void {
    this.pararPolling();
    this.pararTemporizador();
  }

  private initializeForm(): void {
    this.checkoutForm = this.fb.group({
      nome: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      telefone: ['', [Validators.required, Validators.pattern(/^\(\d{2}\) \d{5}-\d{4}$/)]],
      //cpf: ['', Validators.required]
    });
  }

  private carregarPedido(): void {
    this.pedido = this.pedidoService.carregarPedido();
    console.log(this.pedido);

    if (!this.pedido || !this.pedido.itens) {
      this.continuarComprando();
    }

    this.calcularTotais();
  }

  getTotalQuantity(): number {
    let total = this.pedido.itens.reduce((total, item) => total + item.quantidade, 0);
    return total;
  }

  private calcularTotais(): void {
    this.subtotal = this.pedido.subtotal;
    this.frete = this.pedido.frete;

    // Aplicar desconto se houver
    if (this.pedido.descontoAplicado && this.pedido.descontoAplicado > 0) {
      this.descontoAplicado = true;
      this.valorDesconto = this.pedido.descontoAplicado;
      this.total = this.subtotal + this.frete - this.valorDesconto;
    } else {
      this.total = this.subtotal + this.frete;
    }
  }

  // Navegação entre etapas
  nextStep(): void {
    if (this.currentStep < 3) {
      this.currentStep++;

      if (this.currentStep === 3) {
        this.pedido.email = this.email;
        this.pedido.telefone = this.telefone;
        this.gerarPagamentoPix();
      }
    }
  }

  prevStep(): void {
    if (this.currentStep > 1) {
      this.currentStep--;
    }
  }

  isStep1Valid(): boolean {
    if (this.checkoutForm.get('email')?.valid &&
           this.checkoutForm.get('nome')?.valid) {
      return true;
    }
    return false;
  }

  // PIX
  private gerarPagamentoPix(): void {
    this.isLoading = true;
    this.loadingMessage = 'Processando pagamento...';
    this.loadingSubMessage = 'Gerando código PIX';
    console.log(this.currentStep);

    this.pedido.nome = this.f.nome.value;
    this.pedido.email = this.f.email.value;
    this.pedido.telefone = this.f.telefone.value;
    //this.pedido.cpf = this.f.cpf.value;

    this.pedidoService.generatePixPayment(this.pedido).subscribe({
      next: (response: PedidoResponse) => {
        this.isLoading = false;
        this.pedidoResponse = response;

        // Validar se o expirationTime é válido
        const expiracao = new Date(response.expirationTime).getTime();
        if (expiracao <= Date.now()) {
          this.mostrarToast('error', 'Erro no pagamento. Tente novamente.');
          return;
        }

        // Limpar carrinho imediatamente após gerar PIX
        this.cartService.limparCarrinho();
        this.pedidoService.limparPedido();

        // Configurar temporizador
        this.iniciarTemporizador(response.expirationTime);

        // Iniciar polling do status
        this.iniciarPolling(response.transactionId);

        // Abrir modal PIX
        //this.abrirModalPix();
      },
      error: (error) => {
        this.isLoading = false;
        this.mostrarToast('error', 'Erro ao processar pagamento. Tente novamente');
        this.prevStep();
      }
    });
  }

  continuarComprando(): void {
    this.router.navigate(['/produtos']);
  }

  //Temporizador
  iniciarTemporizador(expirationTime: string): void {
    const expiracao = new Date(expirationTime).getTime();
    const agora = Date.now();

    this.tempoTotal = expiracao - agora;
    this.tempoRestante = this.tempoTotal;

    console.log(this.tempoTotal);

    // Verificar se já expirou
    if (this.tempoRestante <= 0) {
      this.progresso = 0;
      return;
    }

    this.pararTemporizador();

    this.intervalId = setInterval(() => {
      const tempoAtual = Date.now();
      this.tempoRestante = expiracao - tempoAtual;
      this.progresso = (this.tempoRestante / this.tempoTotal) * 100;

      // Parar quando expirar
      if (this.tempoRestante <= 0) {
        this.pararTemporizador();
        this.progresso = 0;
        this.tempoRestante = 0;
      }
    }, 1000);
  }

  pararTemporizador(): void {
    if (this.intervalId) {
      clearInterval(this.intervalId);
      this.intervalId = null;
    }
  }

  //Polling
  iniciarPolling(transactionId: string): void {
    this.pararPolling();

    this.paymentPollingSubscription = this.pedidoService.startPaymentPolling(transactionId).subscribe({
      next: (response: PedidoResponse) => {
        this.pedidoResponse = response;

        //FLUXO DE MENSAGENS CONFORME STATUS
        if (response.status === 'paid') {

          // PAGO - DISPARAR EVENTO PURCHASE
          this.dispararEventoPurchase(response);

          // PAGO - para polling e limpa carrinho
          this.pararPolling();
          this.pararTemporizador();
          this.cartService.limparCarrinho();

        } else if (response.status === 'expired' || response.status === 'failed') {
          // EXPIRADO/FALHOU - apenas para polling
          this.pararPolling();

        } else if (response.status === 'pending') {
          // PENDENTE - verifica se expirou pelo tempo
          const expirado = Date.now() >= new Date(response.expirationTime).getTime();
          if (expirado) {
            this.pararPolling();
            // Atualiza status para expirado no frontend
            this.pedidoResponse.status = 'expired';
            console.log(this.pedidoResponse.status);

          }
        }
      },
      error: (error) => {
        console.error('Erro no polling:', error);
        this.pararPolling();
      }
    });
  }

  pararPolling(): void {
    if (this.paymentPollingSubscription) {
      this.paymentPollingSubscription.unsubscribe();
      this.paymentPollingSubscription = undefined;
    }
  }

  isPixPago(): boolean {
    return this.pedidoResponse?.status === 'paid';
  }

  isPixExpirado(): boolean {
    return this.pedidoResponse?.status === 'expired' || this.pedidoResponse?.status === 'failed';
  }

  isPixPendente(): boolean {
    return this.pedidoResponse?.status === 'pending';
  }

  private dispararEventoPurchase(pedidoResponse: PedidoResponse): void {
  try {
    console.log('💰 Disparando evento Purchase - Pedido Pago:', pedidoResponse.transactionId);

    // Precisamos dos dados do pedido para o evento
    const pedidoData = {
      id: pedidoResponse.transactionId,
      total: this.total, // Usa o total do componente
      frete: this.frete, // Usa o frete do componente
      itens: this.cartItems // Usa os itens do carrinho
    };

    // Disparar evento Purchase
    this.marketingTagService.dispararPurchase(pedidoData);

  } catch (error) {
    console.error('❌ Erro ao disparar Purchase:', error);
  }
}

  //método copiar CodigoPix:
  copiarCodigoPix(): void {
    if (this.pedidoResponse) {
      navigator.clipboard.writeText(this.pedidoResponse.pixCode).then(() => {
        this.mostrarToast('success', 'Código PIX copiado para a área de transferência!');
      }).catch(err => {
        this.mostrarToast('success', 'Erro ao copiar código. Tente novamente.');
      });
    }
  }

  // ADICIONAR métodos auxiliares para o template:
  formatarTempoRestante(): string {
    if (this.tempoRestante <= 0) {
      return '00:00';
    }

    const segundosTotais = Math.floor(this.tempoRestante / 1000);

    // Se tiver menos de 60 segundos, mostra apenas SS
    if (segundosTotais < 60) {
      return segundosTotais.toString().padStart(2, '0');
    }
    // Se tiver menos de 60 minutos (3600 segundos), mostra MM:SS
    else if (segundosTotais < 3600) {
      const minutos = Math.floor(segundosTotais / 60);
      const segundos = segundosTotais % 60;
      return `${minutos.toString().padStart(2, '0')}:${segundos.toString().padStart(2, '0')}`;
    }
    // Se tiver 60 minutos ou mais, mostra HH:MM:SS
    else {
      const horas = Math.floor(segundosTotais / 3600);
      const minutos = Math.floor((segundosTotais % 3600) / 60);
      const segundos = segundosTotais % 60;
      return `${horas.toString().padStart(2, '0')}:${minutos.toString().padStart(2, '0')}:${segundos.toString().padStart(2, '0')}`;
    }

  }

  //Fomatar campo telefone
  formatarTelefone(event: any): void {
    let value = event.target.value.replace(/\D/g, '');

    if (value.length > 11) {
      value = value.substring(0, 11);
    }

    if (value.length > 0) {
      value = value.replace(/^(\d{2})(\d)/g, '($1) $2');
    }
    if (value.length > 10) {
      value = value.replace(/(\d{5})(\d)/, '$1-$2');
    } else if (value.length > 6) {
      value = value.replace(/(\d{5})(\d)/, '$1-$2');
    }

    this.checkoutForm.get('telefone')?.setValue(value, { emitEvent: false });
  }

  private mostrarToast(type: 'error' | 'success' | 'warning' | 'info', message: string): void {
    this.toastType = type;
    this.toastMessageText = message;
    this.toastMessage.show();
  }

}
