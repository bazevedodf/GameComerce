using GameCommerce.Aplicacao;
using GameCommerce.Aplicacao.Dtos;
using GameCommerce.Aplicacao.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace GameCommerce.Api.Controllers.V1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PedidosController : ControllerBase
    {
        private readonly IPedidoService _pedidoService;
        private readonly ISiteInfoService _siteInfoService;
        private readonly IConfiguration _configuration;
        private SiteInfoDto _siteInfoDto;

        private Util _util;

        public PedidosController(IPedidoService pedidoService, ISiteInfoService siteInfoService, IConfiguration configuration)
        {
            _pedidoService = pedidoService;
            _siteInfoService = siteInfoService;
            _configuration = configuration;
            _util = new Util(_configuration);
        }

        [HttpPost]
        public async Task<ActionResult<PedidoResponseDto>> CriarPedidoPix(PedidoDto pedidoDto)
        {
            try
            {
                var dominio = _util.IdentificarSite(Request);

                if (string.IsNullOrEmpty(dominio))
                {
                    return Unauthorized("Acesso invalido e não autorizado");
                }

                _siteInfoDto = await _siteInfoService.GetByDominioAsync(dominio, apenasAtivos: true);

                if (_siteInfoDto == null)
                {
                    return Unauthorized($"Site não encontrado para o domínio: {dominio}");
                }

                pedidoDto.SiteInfoId = _siteInfoDto.Id;
                pedidoDto.SiteInfo = _siteInfoDto;

                var resultado = await _pedidoService.ProcessarPagamentoPixAsync(pedidoDto);

                return resultado == null
                    ? BadRequest("Erro ao processar pedido")
                    : Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("status/{transactionId}")]
        public async Task<ActionResult<PedidoResponseDto>> ConsultarStatusPedido(string transactionId)
        {
            try
            {
                var dominio = _util.IdentificarSite(Request);

                if (string.IsNullOrEmpty(dominio))
                {
                    return Unauthorized("Acesso invalido e não autorizado");
                }

                _siteInfoDto = await _siteInfoService.GetByDominioAsync(dominio, apenasAtivos: true);

                if (_siteInfoDto == null)
                {
                    return Unauthorized($"Site não encontrado para o domínio: {dominio}");
                }

                var resultado = await _pedidoService.VerificarStatusPagamentoAsync(transactionId, false);


                return resultado == null ? NotFound("Pedido não encontrado") : Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PedidoDto>> BuscarPorId(int id)
        {
            try
            {
                var dominio = Request.Host.Host;
                var siteInfo = await _siteInfoService.GetByDominioAsync(dominio, apenasAtivos: true);

                if (siteInfo == null)
                {
                    return Unauthorized($"Site não encontrado para o domínio: {dominio}");
                }

                var pedido = await _pedidoService.GetByIdAsync(id);
                if (pedido == null) return NotFound("Pedido não encontrado");

                // Verificar se o pedido pertence ao site
                if (pedido.SiteInfoId != siteInfo.Id)
                {
                    return Unauthorized("Pedido não pertence a este domínio");
                }

                return Ok(pedido);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[HttpPut("{id}/status")]
        //public async Task<ActionResult<bool>> AtualizarStatus(int id, string status)
        //{
        //    try
        //    {
        //        var dominio = Request.Host.Host;
        //        var siteInfo = await _siteInfoService.GetByDominioAsync(dominio, apenasAtivos: true);

        //        if (siteInfo == null)
        //        {
        //            return Unauthorized($"Site não encontrado para o domínio: {dominio}");
        //        }

        //        var pedido = await _pedidoService.GetByIdAsync(id);
        //        if (pedido == null) return NotFound("Pedido não encontrado");

        //        if (pedido.SiteInfoId != siteInfo.Id)
        //        {
        //            return Unauthorized("Pedido não pertence a este domínio");
        //        }

        //        var resultado = await _pedidoService.AtualizarStatusPedidoAsync(id, status);
        //        return !resultado ? BadRequest("Erro ao atualizar status") : Ok(resultado);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

        [HttpPost("webhook/pix")]
        public async Task<ActionResult> ProcessarWebhookPix(string webhookData)
        {
            try
            {
                // Webhook não valida domínio (vem do gateway)
                //await _pedidoService.ProcessarWebhookPixAsync(webhookData);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}