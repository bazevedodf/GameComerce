import {HttpClient} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { SiteInfo } from '../model/siteInfo';
import { Observable, of, tap } from 'rxjs';
import { environment } from '@environments/environment';
import { ActivatedRoute } from '@angular/router';

@Injectable({ providedIn: 'root' })
export class SiteInfoService {

  private cachedInfo?: SiteInfo;  
  private apiUrl = environment.apiUrl+'Loja';

  constructor(private http: HttpClient,
              private route: ActivatedRoute
  ) {}

  getSiteInfo(): Observable<SiteInfo> {
    if (this.cachedInfo) {
      return of(this.cachedInfo);
    }

    return this.http.get<SiteInfo>(this.apiUrl + '/siteinfo').pipe(
      tap(data => this.cachedInfo = data)
    );
  }

}
