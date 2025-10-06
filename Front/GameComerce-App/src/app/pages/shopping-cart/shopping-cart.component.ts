import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { CupomService } from '@app/services/cupom.service';
import { Subscription } from 'rxjs';
import { ToastMessageComponent } from 'src/app/components/toast-message/toast-message.component';
import { Cupom } from 'src/app/model/Cupom';
import { Pedido } from 'src/app/model/Pedido';
import { PedidoResponse } from 'src/app/model/PedidoResponse';
import { CartItem, CartService } from 'src/app/services/cart.service';
import { PedidoService } from 'src/app/services/pedido.service';

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
  
  checkoutForm!: FormGroup;
  cartItems: CartItem[] = [];
  email: string = '';
  telefone: string = '';
  cupom: string = '';
  descontoAplicado: boolean = false;
  valorDesconto: number = 0;
  
  // Totais
  subtotal: number = 0;
  frete: number = 0;
  total: number = 0;
  totalItens: number = 0;

  // Loading
  isLoading: boolean = false;
  loadingMessage: string = 'Processando pagamento...';
  loadingSubMessage: string = 'Gerando código PIX';

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

  constructor(
    private cartService: CartService,
    private pedidoService: PedidoService,
    private cupomService: CupomService,
    private router: Router,
    private fb: FormBuilder
  ) {}

  ngOnInit(): void {
    this.initForm();
    this.carregarCarrinho();
  }

  ngOnDestroy(): void {
    this.pararPolling();
    this.pararTemporizador();
  }

  initForm(): void {
    this.checkoutForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      telefone: ['', [Validators.required, Validators.pattern(/^\(\d{2}\) \d{5}-\d{4}$/)]]
    });
  }

  carregarCarrinho(): void {
    this.cartService.cartItems$.subscribe(items => {
      this.cartItems = items;
      this.calcularTotais();
    });
  }

  calcularTotais(): void {
    this.subtotal = this.cartService.calcularSubtotal();
    this.frete = this.cartService.calcularFrete();
    
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

  finalizarCompra(): void {
    // Marcar todos os campos como touched para mostrar erros
    this.markFormGroupTouched();
    
    if (this.checkoutForm.invalid) {
      this.showValidationError();
      return;
    }

    if (this.cartItems.length === 0) {
      this.mostrarToast('error', 'Seu carrinho está vazio. Adicione produtos antes de finalizar a compra.');
      return;
    }
    
    this.gerarPedidoPix();
  }

  private markFormGroupTouched(): void {
    Object.keys(this.checkoutForm.controls).forEach(key => {
      this.checkoutForm.get(key)?.markAsTouched();
    });
  }

  private showValidationError(): void {
    const errors = this.getFormErrors();
  
    if (errors.emailRequired) {
      this.mostrarToast('error', 'Por favor, informe seu email para continuar.');      
    } else if (errors.emailInvalid) {
      this.mostrarToast('error', 'Verifique se o email está correto.');
    } else if (errors.telefoneRequired) {
      this.mostrarToast('error', 'Por favor, informe seu telefone para continuar.');
    } else if (errors.telefoneInvalid) {
      this.mostrarToast('error', 'Verifique se o telefone está correto. Formato: (11) 99999-9999');
    } else {
      this.mostrarToast('error', 'Por favor, corrija os erros no formulário antes de continuar.');
    }
    
    this.toastType = 'error';
    this.toastMessage.show();
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

  private getFormErrors(): any {
    const email = this.checkoutForm.get('email');
    const telefone = this.checkoutForm.get('telefone');
    
    return {
      emailRequired: email?.errors?.['required'],
      emailInvalid: email?.errors?.['email'],
      telefoneRequired: telefone?.errors?.['required'],
      telefoneInvalid: telefone?.errors?.['pattern']
    };
  }

  // NOVO MÉTODO - Gerar pedido PIX
  gerarPedidoPix(): void {
    this.isLoading = true;
    this.loadingMessage = 'Processando pagamento...';
    this.loadingSubMessage = 'Gerando código PIX';

    this.email = this.checkoutForm.get('email')?.value;

    const pedido: Pedido = {
      email: this.checkoutForm.get('email')?.value,  // ⬅️ PEGAR DO FORM
      telefone: this.checkoutForm.get('telefone')?.value, // ⬅️ NOVO CAMPO
      total: this.total,
      frete: this.frete,
      meioPagamento: 'PIX',
      itens: this.cartItems.map(item => item.produto),
      cupom: this.cupom || undefined,
      descontoAplicado: this.descontoAplicado ? this.valorDesconto : undefined
    };

    this.pedidoService.generatePixPayment(pedido).subscribe({
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
        
        // Configurar temporizador
        this.iniciarTemporizador(response.expirationTime);
        
        // Iniciar polling do status
        this.iniciarPolling(response.transactionId);
        
        // Abrir modal PIX
        this.abrirModalPix();
      },
      error: (error) => {
        this.isLoading = false;
        this.mostrarToast('error', 'Erro ao processar pagamento. Tente novamente');
      }
    });
  }

  // NOVO MÉTODO - Temporizador
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

  // NOVO MÉTODO - Polling
  iniciarPolling(transactionId: string): void {
    this.pararPolling();
    
    this.paymentPollingSubscription = this.pedidoService.startPaymentPolling(transactionId).subscribe({
      next: (response: PedidoResponse) => {
        this.pedidoResponse = response;
        
        //FLUXO DE MENSAGENS CONFORME STATUS
        if (response.status === 'paid') {
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

  abrirModalPix(): void {
    const modalElement = document.getElementById('pixModal');
    if (modalElement) {
      const modal = new bootstrap.Modal(modalElement);
      
      // Configurar evento quando modal for fechado
      modalElement.addEventListener('hidden.bs.modal', () => {
        this.router.navigate(['/']);
      });
      
      modal.show();
    }
  }

  // ATUALIZAR método copiarCodigoPix():
  copiarCodigoPix(): void {
    if (this.pedidoResponse) {
      navigator.clipboard.writeText(this.pedidoResponse.pixCode).then(() => {
        this.mostrarToast('error', 'Código PIX copiado para a área de transferência!');
      }).catch(err => {
        this.mostrarToast('error', 'Erro ao copiar código. Tente novamente.');
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

  private mostrarToast(type: 'error' | 'success' | 'warning' | 'info', message: string): void {
    this.toastType = type;
    this.toastMessageText = message;
    this.toastMessage.show();
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

    if (cupom.tipoDesconto === 'percentual') {
      this.valorDesconto = this.subtotal * (cupom.valorDesconto! / 100);
    } else {
      this.valorDesconto = cupom.valorDesconto!;
    }
    
    this.descontoAplicado = true;
    this.calcularTotais();
  }

  getPercentualDesconto(): number {
    if (this.cupomAplicado?.tipoDesconto === 'percentual') {
      return this.cupomAplicado.valorDesconto || 0; // Retorna o percentual direto
    }
    return 0; // Para cupons de valor fixo, não mostra percentual
  }

  public limparCupom(): void {
    this.cupom = '';
    this.cupomAplicado = undefined; // ⬅️ LIMPAR o cupom aplicado
    this.descontoAplicado = false;
    this.valorDesconto = 0;
    this.calcularTotais();
  }
}