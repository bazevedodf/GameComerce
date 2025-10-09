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

        public async Task<ProdutoDto[]> GetByCategoriaAsync(string categoriaSlug, bool includeCategoria = true)
        {
            try
            {
                var produtos = await _produtoPersist.GetByCategoriaAsync(categoriaSlug, includeCategoria);
                if (produtos == null) return null;

                return _mapper.Map<ProdutoDto[]>(produtos);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ProdutoDto[]> GetDestaquesAsync(bool includeCategoria = true)
        {
            try
            {
                var produtos = await _produtoPersist.GetDestaquesAsync(includeCategoria);
                if (produtos == null) return null;

                return _mapper.Map<ProdutoDto[]>(produtos);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ProdutoDto[]> BuscarAsync(string termo, bool includeCategoria = true)
        {
            try
            {
                var produtos = await _produtoPersist.BuscarAsync(termo, includeCategoria);
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

        public async Task<ProdutoDto[]> GetMaisVendidosPorCategoriaAsync(bool includeCategoria = true)
        {
            try
            {
                var produtos = await _produtoPersist.GetMaisVendidosPorCategoriaAsync(includeCategoria);
                if (produtos == null) return null;

                return _mapper.Map<ProdutoDto[]>(produtos);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}