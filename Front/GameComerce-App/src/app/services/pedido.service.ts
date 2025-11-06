import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, interval, switchMap, takeWhile, delay, of, BehaviorSubject } from 'rxjs';
import { Pedido } from '../model/Pedido';
import { PedidoResponse } from '../model/PedidoResponse';
import { CupomService } from './cupom.service';
import { environment } from '@environments/environment';

@Injectable({
  providedIn: 'root'
})
export class PedidoService {

  private apiUrl = environment.apiUrl+'Pedidos';

  private pedidoSubject = new BehaviorSubject<Pedido>({} as Pedido);
  public pedido$ = this.pedidoSubject.asObservable();

  constructor(private http: HttpClient,
              private cupomService: CupomService ) {
                this.carregarPedido();
              }

  // 1. Gerar pagamento PIX
  generatePixPayment(pedido: Pedido): Observable<PedidoResponse> {
    return this.http.post<PedidoResponse>(`${this.apiUrl}`, pedido);
  }

  // 2. Verificar status do pagamento
  checkPaymentStatus(transactionId: string): Observable<PedidoResponse> {
    return this.http.get<PedidoResponse>(`${this.apiUrl}/status/${transactionId}`);
  }

  // 3. Polling automático para verificar pagamento
  startPaymentPolling(transactionId: string): Observable<PedidoResponse> {
    return interval(3000).pipe(
      switchMap(() => this.checkPaymentStatus(transactionId)),
      takeWhile(response => {

        // Continua polling apenas se ainda estiver pendente E não expirado
        const aindaPendente = response.status === 'pending';
        const aindaValido = Date.now() < new Date(response.expirationTime).getTime();

        return aindaPendente && aindaValido;
      }, true)
    );
  }

  // Persistência do pedido no localStorage
  adicionarPedido(pedido: Pedido): void {
    const currentPedido = this.pedidoSubject.value;

    this.pedidoSubject.next(pedido);
    this.salvarPedido();
  }

  limparPedido(): void {
    this.pedidoSubject.next({} as Pedido);
    localStorage.removeItem('Pedido');
  }

  private salvarPedido(): void {
    localStorage.setItem('Pedido', JSON.stringify(this.pedidoSubject.value));
  }

  public carregarPedido(): Pedido {
    const saved = localStorage.getItem('Pedido');
    if (saved) {
      try {
        const items = JSON.parse(saved);
        this.pedidoSubject.next(items);
        return this.pedidoSubject.value;
      } catch (e) {
        console.error('Erro ao carregar pedido:', e);
      }
    }
    return {} as Pedido;
  }

}
