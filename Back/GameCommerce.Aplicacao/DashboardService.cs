using GameCommerce.Aplicacao.Dtos;
using GameCommerce.Aplicacao.Interfaces;

namespace GameCommerce.Aplicacao.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly ISiteInfoService _siteInfoService;
        private readonly IProdutoService _produtoService;
        private readonly IPedidoService _pedidoService;
        private readonly ICupomService _cupomService;
        private readonly IMarketingTagService _marketingTagService;

        public DashboardService(
            ISiteInfoService siteInfoService,
            IProdutoService produtoService,
            IPedidoService pedidoService,
            ICupomService cupomService,
            IMarketingTagService marketingTagService)
        {
            _siteInfoService = siteInfoService;
            _produtoService = produtoService;
            _pedidoService = pedidoService;
            _cupomService = cupomService;
            _marketingTagService = marketingTagService;
        }

        public async Task<DashboardTotalizadorDto> ObterTotalizadorAsync(int? siteInfoId = null)
        {
            try
            {
                var totalizador = new DashboardTotalizadorDto();

                // 1. Total de Sites Ativos (apenas para totais gerais)
                if (!siteInfoId.HasValue)
                {
                    totalizador.TotalSitesAtivos = await _siteInfoService.GetCountAsync(apenasAtivos: true);
                }
                else
                {
                    var site = await _siteInfoService.GetByIdAsync(siteInfoId.Value);
                    totalizador.TotalSitesAtivos = (site != null && site.Ativo) ? 1 : 0;
                }

                // 2. Total de Produtos
                totalizador.TotalProdutos = await _produtoService.GetCountAsync(siteInfoId, apenasAtivos: true);

                // 3. Total de Pedidos e Pedidos Pagos
                totalizador.TotalPedidos = await _pedidoService.GetCountAsync(siteInfoId);
                totalizador.TotalPedidosPagos = await _pedidoService.GetCountPagosAsync(siteInfoId);

                // 4. Total de Cupons
                totalizador.TotalCupons = await _cupomService.GetCountAsync(siteInfoId, apenasAtivos: true);

                // 5. Total de Marketing Tags
                totalizador.TotalMarketingTags = await _marketingTagService.GetCountAsync(siteInfoId, apenasAtivos: true);

                return totalizador;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao obter totalizador do dashboard: {ex.Message}");
            }
        }
    }
}