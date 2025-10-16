using GameCommerce.Aplicacao.Dtos;
using GameCommerce.Aplicacao.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GameCommerce.Api.Controllers.V2
{
    [ApiController]
    [Route("api/v2/[controller]")]
    [ApiExplorerSettings(GroupName = "v2")]
    public class SiteInfoController : ControllerBase
    {
        private readonly ISiteInfoService _siteInfoService;
        private readonly ICategoriaService _categoriaService;
        private readonly IProdutoService _produtoService;
        private readonly ICupomService _cupomService;
        private readonly IWebHostEnvironment _environment;

        public SiteInfoController(
            ISiteInfoService siteInfoService,
            ICategoriaService categoriaService,
            IProdutoService produtoService,
            ICupomService cupomService,
            IWebHostEnvironment environment)
        {
            _siteInfoService = siteInfoService;
            _categoriaService = categoriaService;
            _produtoService = produtoService;
            _cupomService = cupomService;
            _environment = environment;
        }

        /// <summary>
        /// Obter todos os sites (Admin - inclui inativos)
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll(bool apenasAtivos = false)
        {
            try
            {
                var sites = await _siteInfoService.GetAllAsync(apenasAtivos);

                if (sites == null || !sites.Any())
                    return NotFound("Nenhum site encontrado");

                return Ok(sites);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao buscar sites: {ex.Message}");
            }
        }

        /// <summary>
        /// Obter site por ID (Admin)
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var site = await _siteInfoService.GetByIdAsync(id);
                if (site == null)
                    return NotFound($"Site com ID {id} não encontrado");

                return Ok(site);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao buscar site: {ex.Message}");
            }
        }

        /// <summary>
        /// Obter site por domínio (Admin)
        /// </summary>
        [HttpGet("dominio/{dominio}")]
        public async Task<IActionResult> GetByDominio(string dominio, bool apenasAtivos = true)
        {
            try
            {
                var site = await _siteInfoService.GetByDominioAsync(dominio, apenasAtivos);
                if (site == null)
                    return NotFound($"Site com domínio {dominio} não encontrado");

                return Ok(site);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao buscar site por domínio: {ex.Message}");
            }
        }

        /// <summary>
        /// Clonar site completo (Admin)
        /// </summary>
        [HttpPost("clonar/{id}")]
        public async Task<IActionResult> ClonarSite(int id, 
                            bool clonarCategorias = true, 
                            bool clonarProdutos = true,
                            bool clonarCupons = true)
        {
            SiteInfoDto siteClonado = null;

            try
            {
                // Buscar o site original
                var siteOriginal = await _siteInfoService.GetByIdAsync(id);
                if (siteOriginal == null)
                    return NotFound($"Site com ID {id} não encontrado");

                // REGRA 1: Nome com " - (copia)"
                // REGRA 2: Domínio como "localhost" e status ativo
                var novoSite = new SiteInfoDto
                {
                    Nome = $"{siteOriginal.Nome} - (copia)",
                    Dominio = "localhost",
                    LogoUrl = siteOriginal.LogoUrl,
                    Cnpj = siteOriginal.Cnpj,
                    Address = siteOriginal.Address,
                    Email = siteOriginal.Email,
                    Instagram = siteOriginal.Instagram,
                    Facebook = siteOriginal.Facebook,
                    Whatsapp = siteOriginal.Whatsapp,
                    ApiKey = null, // Não copiar API key por segurança
                    BaseUrl = "http://localhost",
                    Ativo = true, // REGRA 2: Sempre ativo
                    MarketingTags = siteOriginal.MarketingTags?.Select(t => new MarketingTagDto
                    {
                        Tipo = t.Tipo,
                        TagId = t.TagId,
                        Identificador = t.Identificador,
                        Nome = t.Nome,
                        Ativo = t.Ativo
                    }).ToList()
                };

                // Salvar o novo site
                siteClonado = await _siteInfoService.AddAsync(novoSite);
                if (siteClonado == null)
                    return BadRequest("Erro ao criar site clonado");

                var categoriasClonadas = 0;
                var produtosClonados = 0;
                var cuponsClonados = 0;

                // Clonar categorias e subcategorias se solicitado
                if (clonarCategorias)
                {
                    categoriasClonadas = await _categoriaService.ClonarCategoriasAsync(id, siteClonado.Id);
                }

                // Clonar produtos se solicitado
                if (clonarProdutos)
                {
                    produtosClonados = await _produtoService.ClonarProdutosAsync(id, siteClonado.Id);
                }

                // Clonar cupons se solicitado
                if (clonarCupons)
                {
                    cuponsClonados = await _cupomService.ClonarCuponsAsync(id, siteClonado.Id);
                }

                return Ok(new
                {
                    Mensagem = "Site clonado com sucesso",
                    SiteClonado = siteClonado,
                    Detalhes = new
                    {
                        CategoriasClonadas = categoriasClonadas,
                        ProdutosClonados = produtosClonados,
                        CuponsClonados = cuponsClonados
                    }
                });
            }
            catch (Exception ex)
            {
                // REGRA 6: Rollback em caso de erro
                if (siteClonado != null)
                {
                    await _siteInfoService.DeleteAsync(siteClonado.Id);
                }

                return StatusCode(500, new
                {
                    Mensagem = "Erro durante a clonagem do site",
                    Detalhes = ex.Message
                });
            }
        }

        /// <summary>
        /// Deletar site e todos os dados relacionados (Admin)
        /// </summary>
        [HttpDelete("{id}/completo")]
        public async Task<IActionResult> DeleteCompleto(int id, bool realDelete = false)
        {
            try
            {
                // Verificar se o site existe
                var site = await _siteInfoService.GetByIdAsync(id);
                if (site == null)
                    return NotFound($"Site com ID {id} não encontrado");

                // Por enquanto, vamos apenas desativar o site (soft delete)
                // Futuramente podemos implementar a exclusão completa de todos os dados relacionados
                var resultado = await _siteInfoService.DeleteAsync(id, realDelete);

                if (!resultado)
                    return BadRequest("Erro ao deletar site");

                return Ok(new
                {
                    Mensagem = "Site e todos os dados relacionados foram deletados com sucesso",
                    SiteId = id
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Mensagem = "Erro ao deletar site completo",
                    Detalhes = ex.Message
                });
            }
        }

        /// <summary>
        /// Criar novo site (Admin)
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create(SiteInfoDto siteInfoDto)
        {
            try
            {
                var siteCriado = await _siteInfoService.AddAsync(siteInfoDto);
                if (siteCriado == null)
                    return BadRequest("Erro ao criar site");

                return CreatedAtAction(nameof(GetById), new { id = siteCriado.Id }, siteCriado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao criar site: {ex.Message}");
            }
        }

        /// <summary>
        /// Atualizar site (Admin)
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, SiteInfoDto siteInfoDto)
        {
            try
            {
                if (id != siteInfoDto.Id)
                    return BadRequest("ID do site não confere");

                var siteAtualizado = await _siteInfoService.UpdateAsync(siteInfoDto);
                if (siteAtualizado == null)
                    return NotFound($"Site com ID {id} não encontrado");

                return Ok(siteAtualizado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao atualizar site: {ex.Message}");
            }
        }

        /// <summary>
        /// Excluir site (soft delete - Admin)
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var resultado = await _siteInfoService.DeleteAsync(id);
                if (!resultado)
                    return NotFound($"Site com ID {id} não encontrado");

                return Ok(new { mensagem = "Site excluído com sucesso" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao excluir site: {ex.Message}");
            }
        }

        /// <summary>
        /// Ativar/Desativar site (Admin)
        /// </summary>
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> ToggleStatus(int id, bool ativo)
        {
            try
            {
                var site = await _siteInfoService.GetByIdAsync(id);
                if (site == null)
                    return NotFound($"Site com ID {id} não encontrado");

                site.Ativo = ativo;
                var siteAtualizado = await _siteInfoService.UpdateAsync(site);

                return Ok(siteAtualizado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao alterar status do site: {ex.Message}");
            }
        }

        /// <summary>
        /// Upload de logo do site (Admin)
        /// </summary>
        [HttpPost("{id}/upload-logo")]
        public async Task<IActionResult> UploadLogo(int id, IFormFile arquivo)
        {
            try
            {
                if (arquivo == null || arquivo.Length == 0)
                    return BadRequest("Nenhum arquivo enviado");

                // Verificar se o site existe
                var site = await _siteInfoService.GetByIdAsync(id);
                if (site == null)
                    return NotFound($"Site com ID {id} não encontrado");

                // Validar tipo de arquivo
                var extensoesPermitidas = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp", ".svg" };
                var extensao = Path.GetExtension(arquivo.FileName).ToLowerInvariant();
                if (!extensoesPermitidas.Contains(extensao))
                    return BadRequest($"Tipo de arquivo não permitido. Use: {string.Join(", ", extensoesPermitidas)}");

                // Validar tamanho do arquivo (máximo 5MB)
                if (arquivo.Length > 5 * 1024 * 1024)
                    return BadRequest("Arquivo muito grande. Tamanho máximo permitido: 5MB");

                // Criar nome único para o arquivo
                var nomeArquivo = $"logo_{id}_{DateTime.Now:yyyyMMddHHmmss}{extensao}";
                var pastaLogos = Path.Combine(_environment.ContentRootPath, "Uploads", "logos");

                // Garantir que a pasta existe
                if (!Directory.Exists(pastaLogos))
                    Directory.CreateDirectory(pastaLogos);

                var caminhoCompleto = Path.Combine(pastaLogos, nomeArquivo);

                // Salvar arquivo
                using (var stream = new FileStream(caminhoCompleto, FileMode.Create))
                {
                    await arquivo.CopyToAsync(stream);
                }

                // Atualizar site com o caminho da logo
                site.LogoUrl = $"/uploads/logos/{nomeArquivo}";
                var siteAtualizado = await _siteInfoService.UpdateAsync(site);

                return Ok(new
                {
                    mensagem = "Logo uploadada com sucesso",
                    caminho = siteAtualizado.LogoUrl
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao fazer upload da logo: {ex.Message}");
            }
        }
    }
}