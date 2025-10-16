using AutoMapper;
using GameCommerce.Aplicacao.Dtos;
using GameCommerce.Aplicacao.Interfaces;
using GameCommerce.Dominio;
using GameCommerce.Persistencia.Interfaces;

namespace GameCommerce.Aplicacao
{
    public class ProdutoService : IProdutoService
    {
        private readonly IProdutoPersist _produtoPersist;
        private readonly IMapper _mapper;

        public ProdutoService(IProdutoPersist produtoPersist, IMapper mapper)
        {
            _produtoPersist = produtoPersist;
            _mapper = mapper;
        }

        public async Task<ProdutoDto> AddAsync(ProdutoNewDto model)
        {
            try
            {
                var produto = _mapper.Map<Produto>(model);
                _produtoPersist.Add(produto);

                if (await _produtoPersist.SaveChangeAsync())
                {
                    var retorno = await _produtoPersist.GetByIdAsync(produto.Id, true);
                    return _mapper.Map<ProdutoDto>(retorno);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ProdutoDto> UpdateAsync(ProdutoDto model)
        {
            try
            {
                var produto = await _produtoPersist.GetByIdAsync(model.Id);
                if (produto == null) return null;

                _mapper.Map(model, produto);
                _produtoPersist.Update(produto);

                if (await _produtoPersist.SaveChangeAsync())
                {
                    var retorno = await _produtoPersist.GetByIdAsync(produto.Id, true);
                    return _mapper.Map<ProdutoDto>(retorno);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var produto = await _produtoPersist.GetByIdAsync(id);
                if (produto == null) return false;

                produto.Ativo = false;
                _produtoPersist.Update(produto);

                return await _produtoPersist.SaveChangeAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        
        public async Task<ProdutoDto> GetByIdAsync(int id, bool includeCategoria = true)
        {
            try
            {
                var produto = await _produtoPersist.GetByIdAsync(id, includeCategoria);
                if (produto == null) return null;

                return _mapper.Map<ProdutoDto>(produto);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ProdutoDto[]> GetAllAsync(bool includeCategoria = true)
        {
            try
            {
                var produtos = await _produtoPersist.GetAllAsync(includeCategoria);
                if (produtos == null) return null;

                return _mapper.Map<ProdutoDto[]>(produtos);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ProdutoDto[]> GetByCategoriaAsync(int siteId, string categoriaSlug, bool includeCategoria = true)
        {
            try
            {
                var produtos = await _produtoPersist.GetByCategoriaAsync(siteId, categoriaSlug, includeCategoria);
                if (produtos == null) return null;

                return _mapper.Map<ProdutoDto[]>(produtos);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ProdutoDto[]> GetDestaquesAsync(int siteId, bool includeCategoria = true)
        {
            try
            {
                var produtos = await _produtoPersist.GetDestaquesAsync(siteId, includeCategoria);
                if (produtos == null) return null;

                return _mapper.Map<ProdutoDto[]>(produtos);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ProdutoDto[]> BuscarAsync(int siteId, string termo, bool includeCategoria = true)
        {
            try
            {
                var produtos = await _produtoPersist.BuscarAsync(siteId, termo, includeCategoria);
                if (produtos == null) return null;

                return _mapper.Map<ProdutoDto[]>(produtos);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ProdutoDto[]> GetByTagAsync(string tag, bool includeCategoria = true)
        {
            try
            {
                var produtos = await _produtoPersist.GetByTagAsync(tag, includeCategoria);
                if (produtos == null) return null;

                return _mapper.Map<ProdutoDto[]>(produtos);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ProdutoDto[]> GetMaisVendidosPorCategoriaAsync(int siteId, bool includeCategoria = true)
        {
            try
            {
                var produtos = await _produtoPersist.GetMaisVendidosPorCategoriaAsync(siteId, includeCategoria);
                if (produtos == null) return null;

                return _mapper.Map<ProdutoDto[]>(produtos);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ProdutoDto[]> GetAllBySiteIdAsync(int siteId, bool includeCategoria = true)
        {
            try
            {
                // Este método será implementado depois na persistência
                var produtos = await _produtoPersist.GetBySiteIdAsync(siteId, includeCategoria);
                if (produtos == null) return null;

                return _mapper.Map<ProdutoDto[]>(produtos);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<int> ClonarProdutosAsync(int siteOrigemId, int siteDestinoId)
        {
            try
            {
                var produtosOriginais = await GetAllBySiteIdAsync(siteOrigemId, false);
                if (produtosOriginais == null || !produtosOriginais.Any())
                    return 0;

                var produtosClonados = 0;

                foreach (var produtoOriginal in produtosOriginais)
                {
                    var novoProduto = new ProdutoNewDto
                    {
                        Nome = produtoOriginal.Nome,
                        Descricao = produtoOriginal.Descricao,
                        Preco = produtoOriginal.Preco,
                        PrecoOriginal = produtoOriginal.PrecoOriginal,
                        Desconto = produtoOriginal.Desconto,
                        Imagem = produtoOriginal.Imagem,
                        Avaliacao = produtoOriginal.Avaliacao,
                        TotalAvaliacoes = produtoOriginal.TotalAvaliacoes,
                        Tags = produtoOriginal.Tags?.ToList(),
                        Ativo = true,
                        EmDestaque = produtoOriginal.EmDestaque,
                        Entrega = produtoOriginal.Entrega,
                        DataCadastro = DateTime.Now,
                        DataAtualizacao = DateTime.Now,
                        CategoriaId = produtoOriginal.CategoriaId,
                        SiteInfoId = siteDestinoId
                    };

                    var produtoClonado = await AddAsync(novoProduto);
                    if (produtoClonado != null)
                        produtosClonados++;
                }

                return produtosClonados;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao clonar produtos: {ex.Message}");
            }
        }
    }
}