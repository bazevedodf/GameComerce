import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { Router, NavigationEnd } from '@angular/router';
import { LayoutService } from './services/layout.service';
import { filter } from 'rxjs/operators';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  title = 'dashboard';
  isSidebarCollapsed = false;
  isMobile = false;
  currentRoute = '';

  // Variáveis para o spinner global
  globalLoading = false;
  loadingMessage = 'Carregando...';
  loadingSubMessage = '';

  constructor(
    private layoutService: LayoutService,
    private router: Router,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit() {
    // Verificar autenticação
    this.checkAuthentication();

    // Layout observables - usar setTimeout para evitar o erro
    this.layoutService.sidebarCollapsed$.subscribe(collapsed => {
      setTimeout(() => {
        this.isSidebarCollapsed = collapsed;
        this.cdr.detectChanges();
      });
    });

    this.layoutService.isMobile$.subscribe(isMobile => {
      setTimeout(() => {
        this.isMobile = isMobile;
        this.cdr.detectChanges();
      });
    });

    // Monitorar mudanças de rota
    this.router.events
      .pipe(
        filter((event): event is NavigationEnd => event instanceof NavigationEnd)
      )
      .subscribe((event: NavigationEnd) => {
        this.currentRoute = event.urlAfterRedirects;
        this.cdr.detectChanges();
      });

    // Definir rota inicial
    this.currentRoute = this.router.url;
  }

  isLoginPage(): boolean {
    return this.currentRoute === '/login';
  }

  private checkAuthentication() {
    const isAuthenticated = localStorage.getItem('user') || sessionStorage.getItem('user');
    
    if (!isAuthenticated && this.router.url !== '/login') {
      this.router.navigate(['/login']);
    }
  }

  onSidebarToggle(isCollapsed: boolean) {
    // Usar setTimeout para evitar o erro
    setTimeout(() => {
      this.isSidebarCollapsed = isCollapsed;
      this.cdr.detectChanges();
    });
  }

  toggleSidebar() {
    this.layoutService.toggleSidebar();
  }

  // Métodos para controlar o spinner global
  showGlobalLoading(message?: string, subMessage?: string): void {
    setTimeout(() => {
      this.globalLoading = true;
      if (message) this.loadingMessage = message;
      if (subMessage) this.loadingSubMessage = subMessage;
      this.cdr.detectChanges();
    });
  }

  hideGlobalLoading(): void {
    setTimeout(() => {
      this.globalLoading = false;
      this.loadingMessage = 'Carregando...';
      this.loadingSubMessage = '';
      this.cdr.detectChanges();
    });
  }
}