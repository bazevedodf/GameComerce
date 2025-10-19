using GameCommerce.Aplicacao.Dtos;

namespace GameCommerce.Aplicacao.Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardTotalizadorDto> ObterTotalizadorAsync(int? siteId = null);
        // Futuro: Task<RelatorioVendasDto> ObterRelatorioVendasAsync(DateTime inicio, DateTime fim, int? siteId = null);
        // Futuro: Task<DashboardMetricasDto> ObterMetricasAsync(int? siteId = null);
    }
}
