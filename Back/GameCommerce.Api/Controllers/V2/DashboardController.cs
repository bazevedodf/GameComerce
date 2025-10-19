using GameCommerce.Aplicacao.Dtos;
using GameCommerce.Aplicacao.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GameCommerce.Api.Controllers.V2
{
    [ApiController]
    [Route("api/v2/[controller]")]
    [ApiExplorerSettings(GroupName = "v2")]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        /// <summary>
        /// Obter totais do dashboard (geral ou por site)
        /// </summary>
        /// <param name="siteInfoId">ID do site (opcional - se não informado, retorna totais gerais)</param>
        [HttpGet("Totalizador")]
        public async Task<ActionResult<DashboardTotalizadorDto>> GetTotalizador(int? siteInfoId = null)
        {
            try
            {
                var totalizador = await _dashboardService.ObterTotalizadorAsync(siteInfoId);
                return Ok(totalizador);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao buscar totais do dashboard: {ex.Message}");
            }
        }
    }
}