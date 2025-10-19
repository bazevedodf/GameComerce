import { Injectable } from '@angular/core';
import { BehaviorSubject, fromEvent } from 'rxjs';
import { debounceTime } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class LayoutService {
  private isMobileSubject = new BehaviorSubject<boolean>(this.checkIsMobile());
  private sidebarCollapsedSubject = new BehaviorSubject<boolean>(this.checkIsMobile());
  
  isMobile$ = this.isMobileSubject.asObservable();
  sidebarCollapsed$ = this.sidebarCollapsedSubject.asObservable();

  constructor() {
    // Observar mudanças no tamanho da tela
    fromEvent(window, 'resize')
      .pipe(debounceTime(100))
      .subscribe(() => {
        const isMobile = this.checkIsMobile();
        this.isMobileSubject.next(isMobile);
        
        // Se mudou para mobile, recolher sidebar automaticamente
        if (isMobile) {
          this.sidebarCollapsedSubject.next(true);
        }
      });
  }

  private checkIsMobile(): boolean {
    return window.innerWidth < 992; // Bootstrap lg breakpoint
  }

  toggleSidebar() {
    this.sidebarCollapsedSubject.next(!this.sidebarCollapsedSubject.value);
  }

  setSidebarCollapsed(collapsed: boolean) {
    this.sidebarCollapsedSubject.next(collapsed);
  }

  getCurrentSidebarState(): boolean {
    return this.sidebarCollapsedSubject.value;
  }

  getCurrentMobileState(): boolean {
    return this.isMobileSubject.value;
  }
}