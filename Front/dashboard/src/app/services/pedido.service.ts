// pedido.service.ts
import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Pedido } from '@app/models/Pedido';
import { PagedResponse } from '@app/models/Pagination';
import { environment } from '@environments/environment';

@Injectable({
  providedIn: 'root'
})
export class PedidoService {
  private apiUrl = `${environment.apiUrl}pedidos`;

  constructor(private http: HttpClient) { }

  getAll(): Observable<Pedido[]> {
    return this.http.get<Pedido[]>(this.apiUrl);
  }

  getById(id: number): Observable<Pedido> {
    return this.http.get<Pedido>(`${this.apiUrl}/${id}`);
  }

  getPaginatedBySite(siteInfoId: number, page: number = 1, pageSize: number = 10): Observable<PagedResponse<Pedido>> {
    const params = new HttpParams()
      .set('page', page.toString())
      .set('pageSize', pageSize.toString())
      .set('includeItens', 'true');

    return this.http.get<PagedResponse<Pedido>>(`${this.apiUrl}/site/paginado/${siteInfoId}`, { params });
  }

  delete(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }
}