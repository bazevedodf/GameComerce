import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AppComponent } from '@app/app.component';
import { DashboardTotalizador } from '@app/models/DashboardTotalizador';
import { Pagination } from '@app/models/Pagination';
import { SiteConsolidado } from '@app/models/SiteConsolidado';
import { DashboardService } from '@app/services/dashboard.service';
import { SiteInfoService } from '@app/services/siteInfo.service';
import { debounceTime, Subject } from 'rxjs';

declare var bootstrap: any;

@Component({
  selector: 'app-site-lista',
  templateUrl: './site-lista.component.html',
  styleUrls: ['./site-lista.component.scss']
})
export class SiteListaComponent implements OnInit {

  sites: SiteConsolidado[] = [];
  pagination: Pagination = {
    currentPage: 1,
    totalPages: 0,
    totalItems: 0,
    pageSize: 10,
    hasPrevious: false,
    hasNext: false,
  };

  stats: DashboardTotalizador = {
    totalSitesAtivos: 0,
    totalProdutos: 0,
    totalPedidos: 0,
    totalPedidosPagos: 0,
    totalCupons: 0,
    totalMarketingTags: 0
  };

  loading = false;
  searchTerm = '';
  apenasAtivos = false;

  // VARIÁVEIS PARA OS MODAIS
  siteToUpdate: SiteConsolidado | null = null;
  siteToClone: SiteConsolidado | null = null;
  cloneCategorias: boolean = true;
  cloneProdutos: boolean = true;
  cloneCupons: boolean = true;

  searching: boolean = false;
  termoBuscaChanged: Subject<string> = new Subject<string>();

  constructor(
    private siteService: SiteInfoService,
    private dashboardService: DashboardService,
    private appComponent: AppComponent,
    private router: Router
  ) {}

  ngOnInit() {
    this.loadSites();
    this.setupSearchDebounce();
  }

  loadStats(): void {
    this.dashboardService.getTotalizador().subscribe({
      next: (data) => {
        this.stats = data;
      },
      error: (error) => {
        console.error('Erro ao carregar estatísticas:', error);
      }
    });
  }

  loadSites(): void {
    this.appComponent.showGlobalLoading('Carregando sites...','Aguarde um momento');

    this.siteService
      .getSitesConsolidados(
        this.pagination.currentPage,
        this.pagination.pageSize,
        this.apenasAtivos,
        this.searchTerm
      )
      .subscribe({
        next: (response) => {
          this.sites = response.data;
          this.pagination = response.pagination;
          this.loading = false;
          this.loadStats();
        },
        error: (error) => {
          console.error('Erro ao carregar sites:', error);
          this.loading = false;
        },
      }).add(() => { this.appComponent.hideGlobalLoading(); });
  }

  setupSearchDebounce(): void {
    if (this.termoBuscaChanged.observers.length === 0) {
      this.termoBuscaChanged
        .pipe(debounceTime(1000))
        .subscribe((filtrarPor) => {
          this.searchTerm = filtrarPor;
          this.pagination.currentPage = 1;
          this.loadSites();
        });
    }
  }

  // MÉTODO PARA QUANDO O USUÁRIO DIGITA (OPCIONAL)
  onSearchInput(event: any): void {
    this.termoBuscaChanged.next(event.target.value);
  }

  openStatusModal(site: SiteConsolidado): void {
    this.siteToUpdate = site;
    const modalElement = document.getElementById('confirmStatusModal');
    if (modalElement) {
      const modal = new bootstrap.Modal(modalElement);
      modal.show();
    }
  }

  confirmToggleStatus(): void {
    if (!this.siteToUpdate) return;

    const novoStatus = !this.siteToUpdate.status;
    
    this.siteService.toggleSiteStatus(this.siteToUpdate.id, novoStatus).subscribe({
      next: () => {
        // Fechar modal
        const modalElement = document.getElementById('confirmStatusModal');
        if (modalElement) {
          const modal = bootstrap.Modal.getInstance(modalElement);
          modal.hide();
        }
        
        // Atualizar lista
        this.loadSites();
        
        // Limpar variável
        this.siteToUpdate = null;
      },
      error: (error) => {
        console.error('Erro ao atualizar status:', error);
        alert('Erro ao atualizar status do site: ' + error.message);
      }
    });
  }

  openCloneModal(site: SiteConsolidado): void {
    this.siteToClone = site;
    this.cloneCategorias = true;
    this.cloneProdutos = true;
    this.cloneCupons = true;
    
    const modalElement = document.getElementById('cloneSiteModal');
    if (modalElement) {
      const modal = new bootstrap.Modal(modalElement);
      modal.show();
    }
  }

  confirmClone(): void {
    debugger;

    if (!this.siteToClone) 
      return;
    
    this.appComponent.showGlobalLoading('Clonando o site...','Aguarde um momento');
    this.siteService.cloneSite(
      this.siteToClone.id,
      this.cloneCategorias,
      this.cloneProdutos,
      this.cloneCupons
    ).subscribe({
      next: (response) => {
        // Fechar modal
        const modalElement = document.getElementById('cloneSiteModal');
        if (modalElement) {
          const modal = bootstrap.Modal.getInstance(modalElement);
          modal.hide();
        }
        
        // Recarregar lista para mostrar o novo site
        this.loadSites();
        
        // Limpar variáveis
        this.siteToClone = null;
        
        console.log('Site clonado com sucesso:', response);
        alert('Site clonado com sucesso!');
      },
      error: (error) => {
        console.error('Erro ao clonar site:', error);
        alert('Erro ao clonar site: ' + error.message);
      }
    }).add(() => { this.appComponent.hideGlobalLoading(); });
  }

  editarSite(siteId: number): void {
    this.router.navigate(['/sites/detalhe', siteId]);
  }

  // MÉTODOS EXISTENTES (mantenha os que já tem)
  getPagesArray(): number[] {
    const pages = [];
    const totalPages = this.pagination.totalPages;
    const currentPage = this.pagination.currentPage;

    let startPage = Math.max(1, currentPage - 2);
    let endPage = Math.min(totalPages, startPage + 4);

    if (endPage - startPage < 4) {
      startPage = Math.max(1, endPage - 4);
    }

    for (let i = startPage; i <= endPage; i++) {
      pages.push(i);
    }

    return pages;
  }

  onSearch(): void {
    this.pagination.currentPage = 1;
    this.termoBuscaChanged.next(this.searchTerm);
    this.loadSites();
  }

  onToggleAtivos(): void {
    this.pagination.currentPage = 1;
    this.loadSites();
  }

  onPageChange(page: number): void {
    this.pagination.currentPage = page;
    this.loadSites();
  }

  onClearFilters(): void {
    this.searchTerm = '';
    this.apenasAtivos = false;
    this.pagination.currentPage = 1;
    this.loadSites();
  }

  getStatusText(status: boolean): string {
    return status ? 'Ativo' : 'Inativo';
  }

  getStatusBadgeClass(status: boolean): string {
    return status ? 'bg-success' : 'bg-danger';
  }
}