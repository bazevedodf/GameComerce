import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, interval, switchMap, takeWhile, delay, of } from 'rxjs';
import { Pedido } from '../model/Pedido';
import { PedidoResponse } from '../model/PedidoResponse';
import { CupomService } from './cupom.service';

@Injectable({
  providedIn: 'root'
})
export class PedidoService {

  private apiUrl = 'https://sua-api.com/pix'; // URL da sua API

  //apagar quando api ficar pronta
  mockResponse: PedidoResponse = {
      transactionId: 'TX' + Date.now(),
      qrCodeImage: 'assets/img/qr-code-pix.png',
      pixCode: '00020126860014br.gov.bcb.pix2564pix.ecomovi.com.br/qr/v3/at/71ad1e5e-a49d-4b1f-ab00-f82f78e17e8652040000053039865802BR5925KAPTPAY_TECNOLOGIA_DE_PA66009ARAPONGA562070503***630431BD',
      expirationTime: new Date(Date.now() + 60 * 1000).toISOString(),
      status: 'pending'
    };

  constructor(private http: HttpClient,
              private cupomService: CupomService ) { }

  // 1. Gerar pagamento PIX
  generatePixPayment(pedido: Pedido): Observable<PedidoResponse> {
    //REMOVER QUANDO API ESTIVER PRONTA
    console.log(this.mockResponse);    
    return of(this.mockResponse).pipe(delay(3000)); // Simula 3 segundos de loading
    
    //CÓDIGO REAL - DESCOMENTAR QUANDO API ESTIVER PRONTA
    // return this.http.post<PedidoResponse>(`${this.apiUrl}/generate`, pedido);
  }

  // 2. Verificar status do pagamento
  checkPaymentStatus(transactionId: string): Observable<PedidoResponse> {
    //CÓDIGO MOCK - REMOVER QUANDO API ESTIVER PRONTA
    const statuses = ['pending', 'paid', 'expired'];
    const randomStatus = statuses[Math.floor(Math.random() * statuses.length)];
    
    this.mockResponse.status = randomStatus // randomStatus; // Atualiza o status no mockResponse
    
    return of(this.mockResponse).pipe(delay(1000)); // Simula 1 segundo de consulta
    
    // CÓDIGO REAL - DESCOMENTAR QUANDO API ESTIVER PRONTA
    // return this.http.get<PedidoResponse>(`${this.apiUrl}/status/${transactionId}`);
  }

  // 3. Polling automático para verificar pagamento
  startPaymentPolling(transactionId: string): Observable<PedidoResponse> {
    return interval(5000).pipe( // ⬅️ A cada 5 segundos
      switchMap(() => this.checkPaymentStatus(transactionId)),
      takeWhile(response => {
        // Continua polling apenas se ainda estiver pendente E não expirado
        const aindaPendente = response.status === 'pending';
        const aindaValido = Date.now() < new Date(response.expirationTime).getTime();
        
        return aindaPendente && aindaValido;
      }, true) // ⬅️ Emite o último valor que fez parar
    );
  }

}