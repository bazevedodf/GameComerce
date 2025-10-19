import { Component, OnInit } from '@angular/core';
import { AppComponent } from '@app/app.component';
import { Pagination } from '@app/models/Pagination';
import { Pedido } from '@app/models/Pedido';
import { SiteInfo } from '@app/models/SiteInfo';
import { PedidoService } from '@app/services/pedido.service';
import { SiteInfoService } from '@app/services/siteInfo.service';
import { debounceTime, Subject } from 'rxjs';

declare var bootstrap: any;

@Component({
  selector: 'app-pedidos-lista',
  templateUrl: './pedido-lista.component.html',
  styleUrls: ['./pedido-lista.component.scss']
})
export class PedidoListaComponent implements OnInit {

  pedidos: Pedido[] = [];
  pedidosFiltrados: Pedido[] = [];
  sites: SiteInfo[] = [];
  filteredSites: SiteInfo[] = [];
  
  pagination: Pagination = {
    currentPage: 1,
    totalPages: 0,
    totalItems: 0,
    pageSize: 10,
    hasPrevious: false,
    hasNext: false,
  };

  loading = false;
  selectedSite: SiteInfo | null = null;
  siteSearchTerm = '';
  pedidoSearchTerm = '';
  showSitesDropdown = false;

  // VARIÁVEIS PARA OS MODAIS
  selectedPedido: Pedido | null = null;
  pedidoToDelete: Pedido | null = null;

  // VARIÁVEIS PARA SELEÇÃO MÚLTIPLA
  selectedPedidos: Pedido[] = [];
  selectAll: boolean = false;

  searching: boolean = false;
  siteSearchChanged: Subject<string> = new Subject<string>();
  pedidoSearchChanged: Subject<string> = new Subject<string>();

  constructor(
    private pedidoService: PedidoService,
    private siteService: SiteInfoService,
    private appComponent: AppComponent
  ) {}

  ngOnInit() {
    this.loadPedidos();
    this.setupSearchDebounce();
  }

  loadPedidos(): void {
    // Limpa seleção ao carregar novos dados
    this.selectedPedidos = [];
    this.selectAll = false;

    if (!this.selectedSite) {
      this.pedidos = [];
      this.pedidosFiltrados = [];
      this.pagination.totalItems = 0;
      return;
    }

    this.appComponent.showGlobalLoading('Carregando pedidos...', 'Aguarde um momento');

    this.pedidoService
      .getPaginatedBySite(
        this.selectedSite.id,
        this.pagination.currentPage,
        this.pagination.pageSize
      )
      .subscribe({
        next: (response) => {
          this.pedidos = response.data;
          this.pedidosFiltrados = response.data;
          this.pagination = response.pagination;
          this.aplicarFiltroPedidos();
          this.loading = false;
        },
        error: (error) => {
          console.error('Erro ao carregar pedidos:', error);
          this.loading = false;
        },
      }).add(() => { this.appComponent.hideGlobalLoading(); });
  }

  setupSearchDebounce(): void {
    // Debounce para busca de sites
    if (this.siteSearchChanged.observers.length === 0) {
      this.siteSearchChanged
        .pipe(debounceTime(500))
        .subscribe((searchTerm) => {
          this.searchSites(searchTerm);
        });
    }

    // Debounce para busca de pedidos
    if (this.pedidoSearchChanged.observers.length === 0) {
      this.pedidoSearchChanged
        .pipe(debounceTime(1000))
        .subscribe((searchTerm) => {
          this.pedidoSearchTerm = searchTerm;
          this.aplicarFiltroPedidos();
        });
    }
  }

  onSiteSearchInput(event: any): void {
    this.siteSearchTerm = event.target.value;
    this.showSitesDropdown = true;
    this.siteSearchChanged.next(event.target.value);
  }

  searchSites(searchTerm: string): void {
    if (!searchTerm.trim()) {
      this.filteredSites = [];
      return;
    }

    this.siteService.getByTerm(searchTerm, false)
      .subscribe({
        next: (sites) => {
          this.filteredSites = sites;
        },
        error: (error) => {
          console.error('Erro ao buscar sites:', error);
          this.filteredSites = [];
        }
      });
  }

  selectSite(site: SiteInfo): void {
    this.selectedSite = site;
    this.siteSearchTerm = '';
    this.showSitesDropdown = false;
    this.filteredSites = [];
    this.pagination.currentPage = 1;
    this.loadPedidos();
  }

  clearSiteSelection(): void {
    this.selectedSite = null;
    this.pedidos = [];
    this.pedidosFiltrados = [];
    this.pagination.totalItems = 0;
    this.pedidoSearchTerm = '';
    this.selectedPedidos = [];
    this.selectAll = false;
  }

  onPedidoSearchInput(event: any): void {
    this.pedidoSearchChanged.next(event.target.value);
  }

  onPedidoSearch(): void {
    this.aplicarFiltroPedidos();
  }

  aplicarFiltroPedidos(): void {
    if (!this.pedidoSearchTerm.trim()) {
      this.pedidosFiltrados = this.pedidos;
    } else {
      const termo = this.pedidoSearchTerm.toLowerCase();
      this.pedidosFiltrados = this.pedidos.filter(pedido => 
        pedido.email.toLowerCase().includes(termo) ||
        pedido.telefone.toLowerCase().includes(termo)
      );
    }
    // Limpa seleção quando filtra
    this.selectedPedidos = [];
    this.selectAll = false;
  }

  // MÉTODOS PARA SELEÇÃO MÚLTIPLA
  togglePedidoSelection(pedido: Pedido): void {
    const index = this.selectedPedidos.findIndex(p => p.id === pedido.id);
    if (index > -1) {
      this.selectedPedidos.splice(index, 1);
    } else {
      this.selectedPedidos.push(pedido);
    }
  }

  isPedidoSelected(pedido: Pedido): boolean {
    return this.selectedPedidos.some(p => p.id === pedido.id);
  }

  toggleSelectAll(): void {
    if (this.selectAll) {
      this.selectedPedidos = [...this.pedidosFiltrados];
    } else {
      this.selectedPedidos = [];
    }
  }

  onRowClick(pedido: Pedido, event: MouseEvent): void {
    // Só abre o modal se não clicou no checkbox ou ações
    const target = event.target as HTMLElement;
    if (!target.closest('input[type="checkbox"]') && !target.closest('.btn-group')) {
      this.openDetailModal(pedido);
    }
  }

  // Método para selecionar/deselecionar todos
  onSelectAllChange(): void {
    this.selectAll = !this.selectAll;
    this.toggleSelectAll();
  }

  // MODAL PARA EXCLUIR MÚLTIPLOS
  openDeleteMultipleModal(): void {
    if (this.selectedPedidos.length === 0) return;
    
    const modalElement = document.getElementById('confirmDeleteMultipleModal');
    if (modalElement) {
      const modal = new bootstrap.Modal(modalElement);
      modal.show();
    }
  }

  confirmDeleteMultiple(): void {
    if (this.selectedPedidos.length === 0) return;

    this.appComponent.showGlobalLoading('Excluindo pedidos...', `Excluindo ${this.selectedPedidos.length} pedidos`);

    // Cria um array de promises para deletar todos
    const deletePromises = this.selectedPedidos.map(pedido => 
      this.pedidoService.delete(pedido.id).toPromise()
    );

    Promise.all(deletePromises)
      .then(() => {
        // Fechar modal
        const modalElement = document.getElementById('confirmDeleteMultipleModal');
        if (modalElement) {
          const modal = bootstrap.Modal.getInstance(modalElement);
          modal.hide();
        }
        
        // Recarregar lista
        this.loadPedidos();
        
        // Limpar seleção
        this.selectedPedidos = [];
        this.selectAll = false;
        
        console.log(`${this.selectedPedidos.length} pedidos excluídos com sucesso`);
      })
      .catch(error => {
        console.error('Erro ao excluir pedidos:', error);
        alert('Erro ao excluir alguns pedidos: ' + error.message);
      })
      .finally(() => {
        this.appComponent.hideGlobalLoading();
      });
  }

  // MODAIS INDIVIDUAIS
  openDetailModal(pedido: Pedido): void {
    this.selectedPedido = pedido;
    const modalElement = document.getElementById('pedidoDetailModal');
    if (modalElement) {
      const modal = new bootstrap.Modal(modalElement);
      modal.show();
    }
  }

  openDeleteModal(pedido: Pedido): void {
    this.pedidoToDelete = pedido;
    const modalElement = document.getElementById('confirmDeleteModal');
    if (modalElement) {
      const modal = new bootstrap.Modal(modalElement);
      modal.show();
    }
  }

  confirmDelete(): void {
    if (!this.pedidoToDelete) return;

    this.appComponent.showGlobalLoading('Excluindo pedido...', 'Aguarde um momento');

    this.pedidoService.delete(this.pedidoToDelete.id).subscribe({
      next: () => {
        // Fechar modal
        const modalElement = document.getElementById('confirmDeleteModal');
        if (modalElement) {
          const modal = bootstrap.Modal.getInstance(modalElement);
          modal.hide();
        }
        
        // Atualizar lista
        this.loadPedidos();
        
        // Limpar variável
        this.pedidoToDelete = null;
      },
      error: (error) => {
        console.error('Erro ao excluir pedido:', error);
        alert('Erro ao excluir pedido: ' + error.message);
      }
    }).add(() => { this.appComponent.hideGlobalLoading(); });
  }

  // PAGINAÇÃO
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

  onPageChange(page: number): void {
    this.pagination.currentPage = page;
    this.loadPedidos();
  }

  // STATUS BADGES
  getStatusBadgeClass(status: string | undefined): string {
    switch (status) {
      case 'paid':
        return 'bg-success';
      case 'pending':
        return 'bg-warning';
      case 'expired':
        return 'bg-danger';
      default:
        return 'bg-secondary';
    }
  }
}