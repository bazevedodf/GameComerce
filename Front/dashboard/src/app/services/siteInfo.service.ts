import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of, tap } from 'rxjs';
import { environment } from '@environments/environment';
import { SiteConsolidado } from '@app/models/SiteConsolidado';
import { PagedResponse } from '@app/models/Pagination';
import { SiteInfo } from '@app/models/SiteInfo';

@Injectable({ providedIn: 'root' })
export class SiteInfoService {
  private cachedSites?: PagedResponse<SiteConsolidado>;
  private apiUrl = environment.apiUrl + 'SiteInfo';

  constructor(private http: HttpClient) {}

  // Buscar sites por termo
  getByTerm(termo: string, apenasAtivos: boolean = true): Observable<SiteInfo[]> {
  const params = new HttpParams()
    .set('termo', termo)
    .set('apenasAtivos', apenasAtivos.toString());

  return this.http.get<SiteInfo[]>(`${this.apiUrl}/buscar`, { params });
}
 
  // Buscar sites consolidados (lista)
  getSitesConsolidados(
                        page: number = 1,
                        pageSize: number = 10,
                        apenasAtivos: boolean = false,
                        search?: string
                      ): Observable<PagedResponse<SiteConsolidado>> {
    
    let params = new HttpParams()
                      .set('page', page.toString())
                      .set('pageSize', pageSize.toString())
                      .set('apenasAtivos', apenasAtivos.toString());

    if (search && search.trim()) {
      params = params.set('search', search.trim());
    }

    return this.http.get<PagedResponse<SiteConsolidado>>(
      `${this.apiUrl}/SitesTotalizados`,
      { params }
    ).pipe(
      tap(response => {
        if (page === 1 && !apenasAtivos && !search) {
          this.cachedSites = response;
        }
      })
    );
  }

  // CLONAR SITE
  cloneSite(
    siteId: number, 
    clonarCategorias: boolean = true, 
    clonarProdutos: boolean = true, 
    clonarCupons: boolean = true
  ): Observable<any> {
    const params = new HttpParams()
      .set('clonarCategorias', clonarCategorias.toString())
      .set('clonarProdutos', clonarProdutos.toString())
      .set('clonarCupons', clonarCupons.toString());

    return this.http.post(`${this.apiUrl}/clonar/${siteId}`, null, { params });
  }

  // ATIVAR/DESATIVAR SITE
  toggleSiteStatus(siteId: number, ativo: boolean): Observable<any> {
    return this.http.patch(`${this.apiUrl}/${siteId}/status`, { ativo });
  }

  // BUSCAR SITE POR ID (para edição)
  getSiteById(id: number): Observable<SiteInfo> {
    return this.http.get<SiteInfo>(`${this.apiUrl}/${id}`);
  }

  // ATUALIZAR SITE
  updateSite(siteData: SiteInfo): Observable<SiteInfo> {
    if (siteData.id && siteData.id > 0) {
      // Update
      return this.http.put<SiteInfo>(`${this.apiUrl}/${siteData.id}`, siteData);
    } else {
      // Create
      return this.http.post<SiteInfo>(`${this.apiUrl}`, siteData);
    }
  }

  // Limpar cache
  clearCache(): void {
    this.cachedSites = undefined;
  }
}
