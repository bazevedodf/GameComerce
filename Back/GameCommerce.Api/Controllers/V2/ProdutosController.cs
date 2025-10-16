using GameCommerce.Aplicacao.Dtos;
using GameCommerce.Aplicacao.Interfaces;
using GameCommerce.Dominio;
using Microsoft.AspNetCore.Mvc;

namespace GameCommerce.Api.Controllers.V2
{
    [ApiController]
    [Route("api/v2/[controller]")]
    [ApiExplorerSettings(GroupName = "v2")]
    public class ProdutosController : ControllerBase
    {
        private readonly IProdutoService _produtoService;
        private readonly IWebHostEnvironment _environment;

        public ProdutosController(IProdutoService produtoService, IWebHostEnvironment environment)
        {
            _produtoService = produtoService;
            _environment = environment;
        }

        /// <summary>
        /// Duplicar produto (Admin)
        /// </summary>
        [HttpPost("duplicar/{id}")]
        public async Task<IActionResult> Duplicar(int id)
        {
            try
            {
                // Buscar o produto original
                var produtoOriginal = await _produtoService.GetByIdAsync(id);
                if (produtoOriginal == null)
                    return NotFound($"Produto com ID {id} não encontrado");

                // Criar uma cópia do produto
                var produtoCopia = new ProdutoNewDto
                {
                    Nome = $"{produtoOriginal.Nome} - Cópia",
                    Descricao = produtoOriginal.Descricao,
                    Preco = produtoOriginal.Preco,
                    PrecoOriginal = produtoOriginal.PrecoOriginal,
                    Desconto = produtoOriginal.Desconto,
                    Imagem = produtoOriginal.Imagem,
                    Avaliacao = produtoOriginal.Avaliacao ?? 0,
                    TotalAvaliacoes = 0, // Zera as avaliações na cópia
                    Tags = produtoOriginal.Tags?.ToList(), // Cria nova lista
                    Ativo = false, // Deixa inativo por padrão
                    EmDestaque = false, // Remove do destaque
                    Entrega = produtoOriginal.Entrega,
                    DataCadastro = DateTime.Now,
                    DataAtualizacao = DateTime.Now,
                    CategoriaId = produtoOriginal.CategoriaId,
                    SiteInfoId = produtoOriginal.SiteInfoId
                };

                // Salvar a cópia
                var produtoDuplicado = await _produtoService.AddAsync(produtoCopia);
                if (produtoDuplicado == null)
                    return BadRequest("Erro ao duplicar produto");

                return Ok(new
                {
                    mensagem = "Produto duplicado com sucesso",
                    produtoOriginalId = id,
                    produtoDuplicadoId = produtoDuplicado.Id,
                    produto = produtoDuplicado
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao duplicar produto: {ex.Message}");
            }
        }

        /// <summary>
        /// Obter todos os produtos (Admin - inclui inativos)
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll(int siteInfoId, bool apenasAtivos = false)
        {
            try
            {
                var produtos = await _produtoService.GetAllBySiteIdAsync(siteInfoId, apenasAtivos);

                // Filtro adicional para admin
                if (apenasAtivos)
                    produtos = produtos?.Where(p => p.Ativo).ToArray();

                if (produtos == null || !produtos.Any())
                    return NotFound("Nenhum produto encontrado");

                return Ok(produtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao buscar produtos: {ex.Message}");
            }
        }

        /// <summary>
        /// Obter produto por ID (Admin)
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
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
                return StatusCode(500, $"Erro ao buscar produto: {ex.Message}");
            }
        }

        /// <summary>
        /// Criar novo produto (Admin)
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create(ProdutoNewDto produtoDto)
        {
            try
            {
                var produtoCriado = await _produtoService.AddAsync(produtoDto);
                if (produtoCriado == null)
                    return BadRequest("Erro ao criar produto");

                return CreatedAtAction(nameof(GetById), new { id = produtoCriado.Id }, produtoCriado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao criar produto: {ex.Message}");
            }
        }

        /// <summary>
        /// Atualizar produto (Admin)
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ProdutoDto produtoDto)
        {
            try
            {
                if (id != produtoDto.Id)
                    return BadRequest("ID do produto não confere");

                var produtoAtualizado = await _produtoService.UpdateAsync(produtoDto);
                if (produtoAtualizado == null)
                    return NotFound($"Produto com ID {id} não encontrado");

                return Ok(produtoAtualizado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao atualizar produto: {ex.Message}");
            }
        }

        /// <summary>
        /// Excluir produto (soft delete - Admin)
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var resultado = await _produtoService.DeleteAsync(id);
                if (!resultado)
                    return NotFound($"Produto com ID {id} não encontrado");

                return Ok(new { mensagem = "Produto excluído com sucesso" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao excluir produto: {ex.Message}");
            }
        }

        /// <summary>
        /// Ativar/Desativar produto (Admin)
        /// </summary>
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> ToggleStatus(int id, [FromBody] bool ativo)
        {
            try
            {
                var produto = await _produtoService.GetByIdAsync(id);
                if (produto == null)
                    return NotFound($"Produto com ID {id} não encontrado");

                produto.Ativo = ativo;
                var produtoAtualizado = await _produtoService.UpdateAsync(produto);

                return Ok(produtoAtualizado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao alterar status do produto: {ex.Message}");
            }
        }

        /// <summary>
        /// Upload de imagem do produto (Admin)
        /// </summary>
        [HttpPost("{id}/upload-imagem")]
        public async Task<IActionResult> UploadImagem(int id, IFormFile arquivo)
        {
            try
            {
                if (arquivo == null || arquivo.Length == 0)
                    return BadRequest("Nenhum arquivo enviado");

                // Verificar se o produto existe
                var produto = await _produtoService.GetByIdAsync(id);
                if (produto == null)
                    return NotFound($"Produto com ID {id} não encontrado");

                // Validar tipo de arquivo
                var extensoesPermitidas = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
                var extensao = Path.GetExtension(arquivo.FileName).ToLowerInvariant();
                if (!extensoesPermitidas.Contains(extensao))
                    return BadRequest($"Tipo de arquivo não permitido. Use: {string.Join(", ", extensoesPermitidas)}");

                // Criar nome único para o arquivo
                var nomeArquivo = $"produto_{id}_{DateTime.Now:yyyyMMddHHmmss}{extensao}";
                var pastaProdutos = Path.Combine(_environment.ContentRootPath, "Uploads", "produtos");
                var caminhoCompleto = Path.Combine(pastaProdutos, nomeArquivo);

                // Garantir que a pasta existe
                if (!Directory.Exists(pastaProdutos))
                    Directory.CreateDirectory(pastaProdutos);

                // Salvar arquivo
                using (var stream = new FileStream(caminhoCompleto, FileMode.Create))
                {
                    await arquivo.CopyToAsync(stream);
                }

                // Atualizar produto com o caminho da imagem
                produto.Imagem = $"/uploads/produtos/{nomeArquivo}";
                var produtoAtualizado = await _produtoService.UpdateAsync(produto);

                return Ok(new
                {
                    mensagem = "Imagem uploadada com sucesso",
                    caminho = produtoAtualizado.Imagem
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao fazer upload da imagem: {ex.Message}");
            }
        }

        /// <summary>
        /// Obter produtos por categoria (Admin)
        /// </summary>
        [HttpGet("categoria/{categoriaSlug}")]
        public async Task<IActionResult> GetByCategoria(int siteInfoId, string categoriaSlug)
        {
            try
            {
                var produtos = await _produtoService.GetByCategoriaAsync(siteInfoId, categoriaSlug);
                if (produtos == null || !produtos.Any())
                    return NotFound($"Nenhum produto encontrado para a categoria: {categoriaSlug}");

                return Ok(produtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao buscar produtos por categoria: {ex.Message}");
            }
        }

        /// <summary>
        /// Obter produtos em destaque (Admin)
        /// </summary>
        [HttpGet("destaques")]
        public async Task<IActionResult> GetDestaques(int siteInfoId)
        {
            try
            {
                var produtos = await _produtoService.GetDestaquesAsync(siteInfoId,true);
                if (produtos == null || !produtos.Any())
                    return NotFound("Nenhum produto em destaque encontrado");

                return Ok(produtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao buscar produtos em destaque: {ex.Message}");
            }
        }

        /// <summary>
        /// Buscar produtos por termo (Admin)
        /// </summary>
        [HttpGet("busca")]
        public async Task<IActionResult> Buscar(int siteInfoId, string termo)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(termo))
                    return BadRequest("Termo de busca não informado");

                var produtos = await _produtoService.BuscarAsync(siteInfoId, termo);
                if (produtos == null || !produtos.Any())
                    return NotFound($"Nenhum produto encontrado para: {termo}");

                return Ok(produtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao buscar produtos: {ex.Message}");
            }
        }

        /// <summary>
        /// Obter produtos por tag (Admin)
        /// </summary>
        [HttpGet("tag/{tag}")]
        public async Task<IActionResult> GetByTag(string tag)
        {
            try
            {
                var produtos = await _produtoService.GetByTagAsync(tag);
                if (produtos == null || !produtos.Any())
                    return NotFound($"Nenhum produto encontrado com a tag: {tag}");

                return Ok(produtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao buscar produtos por tag: {ex.Message}");
            }
        }

        /// <summary>
        /// Obter produtos mais vendidos (Admin)
        /// </summary>
        [HttpGet("mais-vendidos")]
        public async Task<IActionResult> GetMaisVendidos(int siteInfoId)
        {
            try
            {
                var produtos = await _produtoService.GetMaisVendidosPorCategoriaAsync(siteInfoId);
                if (produtos == null || !produtos.Any())
                    return NotFound("Nenhum produto mais vendido encontrado");

                return Ok(produtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao buscar produtos mais vendidos: {ex.Message}");
            }
        }
    }
}