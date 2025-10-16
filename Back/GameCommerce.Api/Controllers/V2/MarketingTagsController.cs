using GameCommerce.Aplicacao.Dtos;
using GameCommerce.Aplicacao.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GameCommerce.Api.Controllers.V2
{
    [ApiController]
    [Route("api/v2/[controller]")]
    [ApiExplorerSettings(GroupName = "v2")]
    public class MarketingTagsController : ControllerBase
    {
        private readonly IMarketingTagService _marketingTagService;

        public MarketingTagsController(IMarketingTagService marketingTagService)
        {
            _marketingTagService = marketingTagService;
        }

        /// <summary>
        /// Obter todas as marketing tags de um site (Admin)
        /// </summary>
        [HttpGet("site/{siteInfoId}")]
        public async Task<IActionResult> GetAllBySite(int siteInfoId, bool apenasAtivos = true)
        {
            try
            {
                var marketingTags = await _marketingTagService.GetBySiteInfoIdAsync(siteInfoId, apenasAtivos);
                if (marketingTags == null || !marketingTags.Any())
                    return NotFound($"Nenhuma marketing tag encontrada para o site ID {siteInfoId}");

                return Ok(marketingTags);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao buscar marketing tags: {ex.Message}");
            }
        }

        /// <summary>
        /// Obter marketing tag por ID de um site específico (Admin)
        /// </summary>
        [HttpGet("site/{siteInfoId}/{id}")]
        public async Task<IActionResult> GetById(int siteInfoId, int id)
        {
            try
            {
                var marketingTag = await _marketingTagService.GetByIdAsync(id);
                // Valida se a tag pertence ao site informado
                if (marketingTag == null || marketingTag.SiteInfoId != siteInfoId)
                    return NotFound($"Marketing tag com ID {id} não encontrada para o site ID {siteInfoId}");

                return Ok(marketingTag);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao buscar marketing tag: {ex.Message}");
            }
        }

        /// <summary>
        /// Obter marketing tags por tipo de um site específico (Admin)
        /// </summary>
        [HttpGet("site/{siteInfoId}/tipo/{tipo}")]
        public async Task<IActionResult> GetByTipo(int siteInfoId, string tipo, bool apenasAtivos = true)
        {
            try
            {
                var marketingTags = await _marketingTagService.GetByTipoAsync(tipo, siteInfoId, apenasAtivos);
                if (marketingTags == null || !marketingTags.Any())
                    return NotFound($"Nenhuma marketing tag do tipo '{tipo}' encontrada para o site ID {siteInfoId}");

                return Ok(marketingTags);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao buscar marketing tags por tipo: {ex.Message}");
            }
        }

        /// <summary>
        /// Obter marketing tags por identificador de um site específico (Admin)
        /// </summary>
        [HttpGet("site/{siteInfoId}/identificador/{identificador}")]
        public async Task<IActionResult> GetByIdentificador(int siteInfoId, string identificador, bool apenasAtivos = true)
        {
            try
            {
                var marketingTags = await _marketingTagService.GetByIdentificadorAsync(identificador, siteInfoId, apenasAtivos);
                if (marketingTags == null || !marketingTags.Any())
                    return NotFound($"Nenhuma marketing tag com identificador '{identificador}' encontrada para o site ID {siteInfoId}");

                return Ok(marketingTags);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao buscar marketing tags por identificador: {ex.Message}");
            }
        }

        /// <summary>
        /// Criar nova marketing tag (Admin)
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create(MarketingTagDto marketingTagDto)
        {
            try
            {
                var marketingTagCriada = await _marketingTagService.AddAsync(marketingTagDto);
                if (marketingTagCriada == null)
                    return BadRequest("Erro ao criar marketing tag");

                return CreatedAtAction(nameof(GetById), new
                {
                    siteInfoId = marketingTagCriada.SiteInfoId,
                    id = marketingTagCriada.Id
                }, marketingTagCriada);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao criar marketing tag: {ex.Message}");
            }
        }

        /// <summary>
        /// Atualizar marketing tag (Admin)
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, MarketingTagDto marketingTagDto)
        {
            try
            {
                if (id != marketingTagDto.Id)
                    return BadRequest("ID da marketing tag não confere");

                var marketingTagAtualizada = await _marketingTagService.UpdateAsync(marketingTagDto);
                if (marketingTagAtualizada == null)
                    return NotFound($"Marketing tag com ID {id} não encontrada");

                return Ok(marketingTagAtualizada);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao atualizar marketing tag: {ex.Message}");
            }
        }

        /// <summary>
        /// Excluir marketing tag (soft delete - Admin)
        /// </summary>
        [HttpDelete("site/{siteInfoId}/{id}")]
        public async Task<IActionResult> Delete(int siteInfoId, int id)
        {
            try
            {
                // Primeiro valida se a tag pertence ao site
                var marketingTag = await _marketingTagService.GetByIdAsync(id);
                if (marketingTag == null || marketingTag.SiteInfoId != siteInfoId)
                    return NotFound($"Marketing tag com ID {id} não encontrada para o site ID {siteInfoId}");

                var resultado = await _marketingTagService.DeleteAsync(id);
                if (!resultado)
                    return BadRequest("Erro ao excluir marketing tag");

                return Ok(new { mensagem = "Marketing tag excluída com sucesso" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao excluir marketing tag: {ex.Message}");
            }
        }

        /// <summary>
        /// Ativar/Desativar marketing tag (Admin)
        /// </summary>
        [HttpPatch("site/{siteInfoId}/{id}/status")]
        public async Task<IActionResult> ToggleStatus(int siteInfoId, int id, bool ativo)
        {
            try
            {
                // Primeiro valida se a tag pertence ao site
                var marketingTag = await _marketingTagService.GetByIdAsync(id);
                if (marketingTag == null || marketingTag.SiteInfoId != siteInfoId)
                    return NotFound($"Marketing tag com ID {id} não encontrada para o site ID {siteInfoId}");

                marketingTag.Ativo = ativo;
                var marketingTagAtualizada = await _marketingTagService.UpdateAsync(marketingTag);

                return Ok(marketingTagAtualizada);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao alterar status da marketing tag: {ex.Message}");
            }
        }
    }
}