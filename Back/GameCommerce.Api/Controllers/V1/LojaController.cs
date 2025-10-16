using GameCommerce.Aplicacao;
using GameCommerce.Aplicacao.Dtos;
using GameCommerce.Aplicacao.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;

namespace GameCommerce.Api.Controllers.V1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class LojaController : ControllerBase
    {
        private readonly ISiteInfoService _siteInfoService;
        private readonly ICategoriaService _categoriaService;
        private readonly ICupomService _cupomService;
        private readonly IConfiguration _configuration;

        private Util _util;

        public LojaController(
            ISiteInfoService siteInfoService,
            ICategoriaService categoriaService,
            IConfiguration configuration,
            ICupomService cupomService)
        {
            _siteInfoService = siteInfoService;
            _categoriaService = categoriaService;
            _configuration = configuration;
            _cupomService = cupomService;
            _util = new Util(_configuration);
        }

        // GET: api/v1/loja/siteinfo
        [HttpGet("siteinfo")]
        public async Task<ActionResult<SiteInfoDto>> GetSiteInfo()
        {
            try
            {
                
                var dominio = _util.IdentificarSite(Request);

                if (string.IsNullOrEmpty(dominio))
                {
                    return Unauthorized("Acesso invalido e não autorizado");
                }

                // 2. Buscar no banco (apenas sites ativos)
                var siteInfo = await _siteInfoService.GetByDominioAsync(dominio, apenasAtivos: true);

                // 3. Se não achou, retorna erro imediatamente
                if (siteInfo == null)
                {
                    return NotFound($"Site não encontrado para o domínio: {dominio}");
                }

                // 4. Retorna os dados do site
                return Ok(siteInfo);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        // GET: api/v1/loja/categorias
        [HttpGet("categorias")]
        public async Task<ActionResult<CategoriaDto[]>> GetCategorias()
        {
            try
            {
                var dominio = _util.IdentificarSite(Request);

                if (string.IsNullOrEmpty(dominio))
                {
                    return Unauthorized("Acesso invalido e não autorizado");
                }

                // 2. Buscar no banco (apenas sites ativos)
                var siteInfo = await _siteInfoService.GetByDominioAsync(dominio, apenasAtivos: true);

                // 3. Se não achou, retorna erro imediatamente
                if (siteInfo == null)
                {
                    return NotFound($"Site não encontrado para o domínio: {dominio}");
                }

                var categorias = await _categoriaService.GetAllBySiteIdAsync(siteInfo.Id, true);
                if (categorias == null || !categorias.Any())
                    return NoContent();

                return Ok(categorias);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        // GET: api/v1/loja/categorias/destaque
        [HttpGet("categorias/destaque")]
        public async Task<ActionResult<CategoriaDto[]>> GetCategoriasDestaque()
        {
            try
            {
                var dominio = _util.IdentificarSite(Request);

                if (string.IsNullOrEmpty(dominio))
                {
                    return Unauthorized("Acesso invalido e não autorizado");
                }

                // 2. Buscar no banco (apenas sites ativos)
                var siteInfo = await _siteInfoService.GetByDominioAsync(dominio, apenasAtivos: true);

                // 3. Se não achou, retorna erro imediatamente
                if (siteInfo == null)
                {
                    return NotFound($"Site não encontrado para o domínio: {dominio}");
                }

                var categorias = await _categoriaService.GetAllBySiteIdAsync(siteInfo.Id, true);
                if (categorias == null || !categorias.Any())
                    return NoContent();

                // Filtrar categorias que têm imagem (critério para destaque)
                var categoriasDestaque = categorias
                    .Where(c => !string.IsNullOrEmpty(c.Imagem) && c.Ativo)
                    .ToArray();

                if (!categoriasDestaque.Any())
                    return NoContent();

                return Ok(categoriasDestaque);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        // GET: api/v1/loja/cupons/validar
        [HttpGet("cupons/validar")]
        public async Task<ActionResult<CupomDto>> ValidarCupom(string codigo)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(codigo))
                    return BadRequest("Código do cupom é obrigatório");

                var cupom = await _cupomService.ValidarCupomAsync(codigo);
                return Ok(cupom);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

    }
}