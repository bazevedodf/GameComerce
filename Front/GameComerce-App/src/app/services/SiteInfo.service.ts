import {HttpClient} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { SiteInfo } from '../model/siteInfo';
import { Observable, of, tap } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class SiteInfoService {

  private cachedInfo?: SiteInfo;  

  constructor(private http: HttpClient) {}

  getSiteInfo(): Observable<SiteInfo> {
    if (this.cachedInfo) {
      return of(this.cachedInfo);
    }

    // Dados fictícios para testes
    const mockInfo: SiteInfo = {
      nome: 'Game Commerce',
      dominio: 'gamecomerce.com.br',
      logoUrl: 'https://ggbundles.com/assets/logo-site-BRumCtgc.png',
      cnpj: '12.345.678/0001-99',
      address: 'Rua dos Games, 123 - São Paulo, SP',
      email: 'contato@gamecomerce.com.br',
      instagram: 'https://instagram.com/gamecomerce',
      facebook: 'https://facebook.com/gamecomerce',
      whatsapp: 'https://wa.me/5511999999999',
    };

    this.cachedInfo = mockInfo;
    return of(mockInfo);

    // return this.http.get<SiteInfo>('https://sua-api.com/site-info').pipe(
    //   tap(data => this.cachedInfo = data)
    // );
  }

}
