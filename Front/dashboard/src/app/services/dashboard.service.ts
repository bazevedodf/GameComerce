import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '@environments/environment';
import { DashboardTotalizador } from '@app/models/DashboardTotalizador';

@Injectable({
  providedIn: 'root'
})
export class DashboardService {
  private apiUrl = environment.apiUrl + 'Dashboard';

  constructor(private http: HttpClient) { }

  getTotalizador(siteInfoId?: number): Observable<DashboardTotalizador> {
    let url = `${this.apiUrl}/Totalizador`;
    
    if (siteInfoId) {
      url += `?siteInfoId=${siteInfoId}`;
    }

    return this.http.get<DashboardTotalizador>(url);
  }
  
}