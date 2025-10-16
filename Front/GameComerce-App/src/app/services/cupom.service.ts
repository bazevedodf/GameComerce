import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError, delay } from 'rxjs/operators';
import { Cupom } from '../model/Cupom';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class CupomService {

  private apiUrl = `${environment.apiUrl}Loja/cupons`;

  // MOCK TEMPORÁRIO - REMOVER QUANDO API FICAR PRONTA
  private cuponsMock: Cupom[] = [
    {
      id: 1,
      codigo: 'PRIMEIRACOMPRA',
      valido: true,
      valorDesconto: 60,
      tipoDesconto: 'percentual',
      mensagemErro: ''
    },
    {
      id: 2,
      codigo: 'FRETEGRATIS',
      valido: true,
      valorDesconto: 15,
      tipoDesconto: 'valor_fixo',
      mensagemErro: ''
    },
    {
      id: 3,
      codigo: 'VBUCKS10',
      valido: true,
      valorDesconto: 5,
      tipoDesconto: 'percentual',
      mensagemErro: ''
    },
    {
      id: 4,
      codigo: 'EXPIRADO',
      valido: false,
      valorDesconto: 0,
      tipoDesconto: 'percentual',
      mensagemErro: 'Este cupom expirou'
    },
    {
      id: 5,
      codigo: 'INVALIDO',
      valido: false,
      valorDesconto: 0,
      tipoDesconto: 'percentual',
      mensagemErro: 'Cupom inválido'
    }
  ];

  constructor(private http: HttpClient) { }

  validarCupom(codigo: string): Observable<Cupom> {
    //CÓDIGO REAL - DESCOMENTAR QUANDO API ESTIVER PRONTA
    return this.http.get<Cupom>(`${this.apiUrl}/validar`, {
      params: { codigo }
    }).pipe(
      catchError(error => {
        console.error('Erro ao validar cupom:', error);
        return of(this.criarCupomInvalido(codigo, 'Erro ao validar cupom. Tente novamente.'));
      })
    );
  }

  // MÉTODOS AUXILIARES (permanecem iguais)
  calcularValorDesconto(cupom: Cupom, subtotal: number): number {
    if (!cupom.valido || !cupom.valorDesconto) {
      return 0;
    }
    if (cupom.tipoDesconto === 'percentual') {
      return subtotal * (cupom.valorDesconto / 100);
    } else {
      return Math.min(cupom.valorDesconto, subtotal);
    }
  }

  private criarCupomInvalido(codigo: string, mensagemErro: string): Cupom {
    return {
      id: 0,
      codigo: codigo.toUpperCase(),
      valido: false,
      valorDesconto: 0,
      tipoDesconto: 'percentual',
      mensagemErro
    };
  }
}