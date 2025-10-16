using GameCommerce.Aplicacao;
using GameCommerce.Aplicacao.Dtos;
using GameCommerce.Aplicacao.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace GameCommerce.Api.Controllers.V1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProdutosController : ControllerBase
    {
        private readonly IProdutoService _produtoService;
        private readonly ISiteInfoService _siteInfoService;
        private readonly ICategoriaService _categoriaService;
        private readonly IConfiguration _configuration;
        private Util _util;

        public ProdutosController(
            IProdutoService produtoService,
            ISiteInfoService siteInfoService,
            ICategoriaService categoriaService,
            IConfiguration configuration)
        {
            _produtoService = produtoService;
            _siteInfoService = siteInfoService;
            _categoriaService = categoriaService;
            _configuration = configuration;
            _util = new Util(_configuration);
        }

        // GET: api/v1/produtos
        [HttpGet]
        public async Task<ActionResult<ProdutoDto[]>> GetAllProdutos()
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

                var produtos = await _produtoService.GetAllBySiteIdAsync(siteInfo.Id, false);
                if (produtos == null || !produtos.Any())
                    return NoContent();

                return Ok(produtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        // GET: api/v1/produtos
        [HttpGet("Destaques")]
        public async Task<ActionResult<ProdutoDto[]>> GetDestaques()
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

                var produtos = await _produtoService.GetMaisVendidosPorCategoriaAsync(siteInfo.Id);
                if (produtos == null || !produtos.Any())
                    return NoContent();

                return Ok(produtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        // GET: api/v1/produtos/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ProdutoDto>> GetProdutoPorId(int id)
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

                var produto = await _produtoService.GetByIdAsync(id);
                if (produto == null)
                    return NotFound($"Produto com ID {id} não encontrado");

                return Ok(produto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        // GET: api/v1/produtos/categoria/{slug}
        [HttpGet("categoria/{slug}")]
        public async Task<ActionResult<ProdutoDto[]>> GetProdutosPorCategoria(string slug)
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

                var produtos = await _produtoService.GetByCategoriaAsync(siteInfo.Id, slug);
                if (produtos == null || !produtos.Any())
                    return NoContent();

                return Ok(produtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        // GET: api/v1/produtos/busca
        [HttpGet("busca")]
        public async Task<ActionResult<ProdutoDto[]>> BuscarProdutos(string termo)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(termo))
                {
                    return BadRequest("Pelo menos um parâmetro de busca deve ser fornecido");
                }

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

                var produtos = await _produtoService.BuscarAsync(siteInfo.Id, termo);

                if (produtos == null || !produtos.Any())
                    return NoContent();

                return Ok(produtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }
    }
}