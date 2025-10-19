using AutoMapper;
using GameCommerce.Aplicacao.Dtos;
using GameCommerce.Aplicacao.Helpers;
using GameCommerce.Aplicacao.Interfaces;
using GameCommerce.Dominio;
using GameCommerce.Persistencia.Interfaces;

namespace GameCommerce.Aplicacao
{
    public class SiteInfoService : ISiteInfoService
    {
        private readonly ISiteInfoPersist _siteInfoPersist;
        private readonly IPedidoService _pedidoService;
        private readonly IMapper _mapper;

        public SiteInfoService(ISiteInfoPersist siteInfoPersist, IPedidoService pedidoService, IMapper mapper)
        {
            _siteInfoPersist = siteInfoPersist;
            _pedidoService = pedidoService;
            _mapper = mapper;
        }

        public async Task<SiteInfoDto> AddAsync(SiteInfoDto model)
        {
            try
            {
                var siteInfo = _mapper.Map<SiteInfo>(model);
                _siteInfoPersist.Add(siteInfo);

                if (await _siteInfoPersist.SaveChangeAsync())
                {
                    var retorno = await _siteInfoPersist.GetByIdAsync(siteInfo.Id);
                    return _mapper.Map<SiteInfoDto>(retorno);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<SiteInfoDto> UpdateAsync(SiteInfoDto model)
        {
            try
            {
                var siteInfo = await _siteInfoPersist.GetByIdAsync(model.Id);
                if (siteInfo == null) return null;

                _mapper.Map(model, siteInfo);
                _siteInfoPersist.Update(siteInfo);

                if (await _siteInfoPersist.SaveChangeAsync())
                {
                    var retorno = await _siteInfoPersist.GetByIdAsync(siteInfo.Id);
                    return _mapper.Map<SiteInfoDto>(retorno);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteAsync(int id, bool realDelete = false)
        {
            try
            {
                var siteInfo = await _siteInfoPersist.GetByIdAsync(id);
                if (siteInfo == null) return false;

                if (realDelete)
                {
                    _siteInfoPersist.Delete(siteInfo);
                }
                else
                {
                    siteInfo.Ativo = false;
                    _siteInfoPersist.Update(siteInfo);
                }

                return await _siteInfoPersist.SaveChangeAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<SiteInfoDto> GetByIdAsync(int id)
        {
            try
            {
                var siteInfo = await _siteInfoPersist.GetByIdAsync(id);
                if (siteInfo == null) return null;

                return _mapper.Map<SiteInfoDto>(siteInfo);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<SiteInfoDto[]> GetByTermAsync(string termo, bool apenasAtivos = true)
        {
            try
            {
                var sites = await _siteInfoPersist.GetByTermAsync(termo, apenasAtivos);
                if (sites == null) return null;

                return _mapper.Map<SiteInfoDto[]>(sites);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao buscar sites: {ex.Message}");
            }
        }

        public async Task<SiteInfoDto> GetByDominioAsync(string dominio, bool apenasAtivos = true)
        {
            try
            {
                var siteInfo = await _siteInfoPersist.GetByDominioAsync(dominio, apenasAtivos);
                if (siteInfo == null) return null;

                return _mapper.Map<SiteInfoDto>(siteInfo);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<SiteInfoDto[]> GetAllAsync(bool apenasAtivos = true)
        {
            try
            {
                var siteInfos = await _siteInfoPersist.GetAllAsync(apenasAtivos);
                if (siteInfos == null) return null;

                return _mapper.Map<SiteInfoDto[]>(siteInfos);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        //Metodos Qantitativos
        public async Task<int> GetCountAsync(bool apenasAtivos = true)
        {
            try
            {
                return await _siteInfoPersist.GetCountAsync(apenasAtivos);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao contar sites: {ex.Message}");
            }
        }

        public async Task<PagedResponse<SiteConsolidadoDto>> GetAllConsolidadoAsync(int page = 1, int pageSize = 10, string? search = null, bool apenasAtivos = true)
        {
            try
            {
                // Buscar dados paginados do banco
                var siteInfos = await _siteInfoPersist.GetAllConsolidadoAsync(page, pageSize, search, apenasAtivos);
                var totalItems = await _siteInfoPersist.GetTotalCountAsync(apenasAtivos);
                var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

                if (siteInfos == null || !siteInfos.Any())
                    return new PagedResponse<SiteConsolidadoDto>
                    {
                        Data = new List<SiteConsolidadoDto>(),
                        Pagination = new PaginationInfo
                        {
                            CurrentPage = page,
                            TotalPages = totalPages,
                            TotalItems = totalItems,
                            PageSize = pageSize
                        }
                    };

                var lista = new List<SiteConsolidadoDto>();
                foreach (var siteInfo in siteInfos)
                {
                    var pedidos = await _pedidoService.GetAllBySiteInfoIdAsync(siteInfo.Id, true);

                    var siteCons = new SiteConsolidadoDto()
                    {
                        Id = siteInfo.Id,
                        Nome = siteInfo.Nome,
                        Dominio = siteInfo.Dominio,
                        Status = siteInfo.Ativo,
                        TotalProdutos = siteInfo.Produtos?.Count() ?? 0,
                        TotalCupons = siteInfo.Cupons?.Count() ?? 0,
                        TotalPedidos = pedidos?.Count() ?? 0,
                        TotalPagos = pedidos?.Where(x =>
                            x.TransacaoPagamento?.Status?.ToLower() == "paid"
                        ).Count() ?? 0
                    };
                    lista.Add(siteCons);
                }

                return new PagedResponse<SiteConsolidadoDto>
                {
                    Data = lista,
                    Pagination = new PaginationInfo
                    {
                        CurrentPage = page,
                        TotalPages = totalPages,
                        TotalItems = totalItems,
                        PageSize = pageSize
                    }
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


    }
}