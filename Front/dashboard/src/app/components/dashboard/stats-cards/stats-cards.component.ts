import { Component, OnInit, Input } from '@angular/core';
import { DashboardTotalizador } from '@app/models/DashboardTotalizador';
import { DashboardService } from '@app/services/dashboard.service';


@Component({
  selector: 'app-stats-cards',
  templateUrl: './stats-cards.component.html',
  styleUrls: ['./stats-cards.component.scss']
})
export class StatsCardsComponent implements OnInit {

  @Input() siteInfoId?: number; // Opcional: para filtrar por site específico
  @Input() autoRefresh: boolean = true; // Se deve atualizar automaticamente

  stats: DashboardTotalizador = {
    totalSitesAtivos: 0,
    totalProdutos: 0,
    totalPedidos: 0,
    totalPedidosPagos: 0,
    totalCupons: 0,
    totalMarketingTags: 0
  };

  loading = false;

  constructor(private dashboardService: DashboardService) { }

  ngOnInit() {
    this.loadStats();
  }

  loadStats(): void {
    this.loading = true;
    
    this.dashboardService.getTotalizador(this.siteInfoId).subscribe({
      next: (data) => {
        this.stats = data;
        this.loading = false;
      },
      error: (error) => {
        //console.error('Erro ao carregar estatísticas:', error);
        this.loading = false;
      }
    });
  }

  // Método público para forçar atualização
  refresh(): void {
    this.loadStats();
  }
}