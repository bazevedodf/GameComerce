using Microsoft.AspNetCore.Mvc;
using GameCommerce.Aplicacao.Interfaces;
using GameCommerce.Aplicacao.Dtos;

namespace GameCommerce.Api.Controllers.V1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProdutosController : ControllerBase
    {
        private readonly IProdutoService _produtoService;
        private readonly ICategoriaService _categoriaService;

        public ProdutosController(
            IProdutoService produtoService,
            ICategoriaService categoriaService)
        {
            _produtoService = produtoService;
            _categoriaService = categoriaService;
        }

        // GET: api/v1/produtos
        [HttpGet]
        public async Task<ActionResult<ProdutoDto[]>> GetAllProdutos()
        {
            try
            {
                var produtos = await _produtoService.GetAllAsync();
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
                var produtos = await _produtoService.GetMaisVendidosPorCategoriaAsync();
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
                var produtos = await _produtoService.GetByCategoriaAsync(slug);
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

                var produtos = await _produtoService.BuscarAsync(termo);

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