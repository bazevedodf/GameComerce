using GameCommerce.Aplicacao.Dtos;
using GameCommerce.Aplicacao.Helpers;
using GameCommerce.Aplicacao.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GameCommerce.Api.Controllers.V2
{
    [ApiController]
    [Route("api/v2/[controller]")]
    [ApiExplorerSettings(GroupName = "v2")]
    public class PedidosController : ControllerBase
    {
        private readonly IPedidoService _pedidoService;

        public PedidosController(IPedidoService pedidoService)
        {
            _pedidoService = pedidoService;
        }

        // GET: api/v2/pedidos
        [HttpGet]
        public async Task<ActionResult<PedidoDto[]>> GetAll()
        {
            try
            {
                var pedidos = await _pedidoService.GetAllAsync();
                if (pedidos == null || !pedidos.Any())
                    return NotFound("Nenhum pedido encontrado");

                return Ok(pedidos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao buscar pedidos: {ex.Message}");
            }
        }

        // GET: api/v2/pedidos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PedidoDto>> GetById(int id)
        {
            try
            {
                var pedido = await _pedidoService.GetByIdAsync(id);
                if (pedido == null)
                    return NotFound($"Pedido com ID {id} não encontrado");

                return Ok(pedido);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao buscar pedido: {ex.Message}");
            }
        }

        // GET: api/v2/pedidos/transacao/TRX-12345
        [HttpGet("transacao/{transactionId}")]
        public async Task<ActionResult<PedidoDto>> GetByTransactionId(string transactionId)
        {
            try
            {
                var pedido = await _pedidoService.GetByTransactionIdAsync(transactionId);
                if (pedido == null)
                    return NotFound($"Pedido com transaction ID {transactionId} não encontrado");

                return Ok(pedido);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao buscar pedido: {ex.Message}");
            }
        }

        // GET: api/v2/pedidos/site/1
        [HttpGet("site/{siteInfoId}")]
        public async Task<ActionResult<PedidoDto[]>> GetBySiteInfoId(int siteInfoId)
        {
            try
            {
                var pedidos = await _pedidoService.GetAllBySiteInfoIdAsync(siteInfoId);
                if (pedidos == null || !pedidos.Any())
                    return NotFound($"Nenhum pedido encontrado para o site ID {siteInfoId}");

                return Ok(pedidos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao buscar pedidos por site: {ex.Message}");
            }
        }

        /// <summary>
        /// Obter pedidos por site com paginação (Admin)
        /// </summary>
        /// <param name="siteInfoId">ID do site</param>
        /// <param name="page">Página atual (padrão: 1)</param>
        /// <param name="pageSize">Itens por página (padrão: 10)</param>
        /// <param name="includeItens">Incluir itens do pedido (padrão: true)</param>
        [HttpGet("site/paginado/{siteInfoId}")]
        public async Task<ActionResult<PagedResponse<PedidoDto>>> GetBySiteInfoIdPaginado(
            int siteInfoId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] bool includeItens = true)
        {
            try
            {
                var pedidosPaginados = await _pedidoService.GetPaginatedBySiteInfoIdAsync(
                    page, pageSize, siteInfoId, includeItens);

                return Ok(pedidosPaginados);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao buscar pedidos paginados: {ex.Message}");
            }
        }

        // PUT: api/v2/pedidos/5
        [HttpPut("{id}")]
        public async Task<ActionResult<PedidoDto>> Update(int id, PedidoDto pedidoDto)
        {
            try
            {
                if (id != pedidoDto.Id)
                    return BadRequest("ID do pedido não confere");

                var pedidoAtualizado = await _pedidoService.UpdateAsync(pedidoDto);
                if (pedidoAtualizado == null)
                    return NotFound($"Pedido com ID {id} não encontrado");

                return Ok(pedidoAtualizado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao atualizar pedido: {ex.Message}");
            }
        }

        // DELETE: api/v2/pedidos/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var resultado = await _pedidoService.DeleteAsync(id);
                if (!resultado)
                    return NotFound($"Pedido com ID {id} não encontrado");

                return Ok(new { mensagem = "Pedido excluído com sucesso" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao excluir pedido: {ex.Message}");
            }
        }
    }
}