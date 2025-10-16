using AutoMapper;
using GameCommerce.Aplicacao.Dtos;
using GameCommerce.Aplicacao.Interfaces;
using GameCommerce.Dominio;
using GameCommerce.Persistencia.Interfaces;

namespace GameCommerce.Aplicacao
{
    public class CategoriaService : ICategoriaService
    {
        private readonly ICategoriaPersist _categoriaPersist;
        private readonly IMapper _mapper;

        public CategoriaService(ICategoriaPersist categoriaPersist, IMapper mapper)
        {
            _categoriaPersist = categoriaPersist;
            _mapper = mapper;
        }

        public async Task<CategoriaDto> AddAsync(CategoriaDto model)
        {
            try
            {
                var categoria = _mapper.Map<Categoria>(model);
                _categoriaPersist.Add(categoria);

                if (await _categoriaPersist.SaveChangeAsync())
                {
                    var retorno = await _categoriaPersist.GetByIdAsync(categoria.Id);
                    return _mapper.Map<CategoriaDto>(retorno);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<CategoriaDto> UpdateAsync(CategoriaDto model)
        {
            try
            {
                var categoria = await _categoriaPersist.GetByIdAsync(model.Id);
                if (categoria == null) return null;

                _mapper.Map(model, categoria);
                _categoriaPersist.Update(categoria);

                if (await _categoriaPersist.SaveChangeAsync())
                {
                    var retorno = await _categoriaPersist.GetByIdAsync(categoria.Id);
                    return _mapper.Map<CategoriaDto>(retorno);
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
                var categoria = await _categoriaPersist.GetByIdAsync(id);
                if (categoria == null) return false;

                categoria.Ativo = false; // Soft delete
                _categoriaPersist.Update(categoria);

                return await _categoriaPersist.SaveChangeAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<CategoriaDto[]> BuscarAsync(string termo)
        {
            try
            {
                var categorias = await _categoriaPersist.BuscarAsync(termo);
                if (categorias == null) return null;

                return _mapper.Map<CategoriaDto[]>(categorias);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<CategoriaDto> GetByIdAsync(int id, bool includeSubcategorias = true)
        {
            try
            {
                var categoria = await _categoriaPersist.GetByIdAsync(id, includeSubcategorias);
                if (categoria == null) return null;

                return _mapper.Map<CategoriaDto>(categoria);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<CategoriaDto> GetBySlugAsync(string slug, bool includeSubcategorias = true)
        {
            try
            {
                var categoria = await _categoriaPersist.GetBySlugAsync(slug, includeSubcategorias); // Include subcategorias
                if (categoria == null) return null;

                return _mapper.Map<CategoriaDto>(categoria);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<CategoriaDto[]> GetAllAsync(bool includeSubcategorias = true)
        {
            try
            {
                var categorias = await _categoriaPersist.GetAllAsync(includeSubcategorias); // Include subcategorias
                if (categorias == null) return null;

                return _mapper.Map<CategoriaDto[]>(categorias);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<CategoriaDto[]> GetAllBySiteIdAsync(int siteId, bool includeSubcategorias = true)
        {
            try
            {
                var categorias = await _categoriaPersist.GetAllBySiteIdAsync(siteId, includeSubcategorias); // Include subcategorias
                if (categorias == null) return null;

                return _mapper.Map<CategoriaDto[]>(categorias);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<int> ClonarCategoriasAsync(int siteOrigemId, int siteDestinoId)
        {
            try
            {
                // Usando o método existente GetAllBySiteIdAsync
                var categoriasOriginais = await GetAllBySiteIdAsync(siteOrigemId, true);
                if (categoriasOriginais == null || !categoriasOriginais.Any())
                    return 0;

                var categoriasClonadas = 0;

                foreach (var categoriaOriginal in categoriasOriginais)
                {
                    var novaCategoria = new CategoriaDto
                    {
                        Name = categoriaOriginal.Name,
                        Slug = categoriaOriginal.Slug,
                        Descricao = categoriaOriginal.Descricao,
                        Imagem = categoriaOriginal.Imagem,
                        Icon = categoriaOriginal.Icon,
                        Ativo = true,
                        SiteInfoId = siteDestinoId,
                        Subcategorias = categoriaOriginal.Subcategorias?
                            .Where(s => s.Ativo)
                            .Select(s => new SubcategoriaDto
                            {
                                Name = s.Name,
                                Slug = s.Slug,
                                Ativo = true
                            }).ToList()
                    };

                    var categoriaClonada = await AddAsync(novaCategoria);
                    if (categoriaClonada != null)
                        categoriasClonadas++;
                }

                return categoriasClonadas;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao clonar categorias: {ex.Message}");
            }
        }

    }
}