using AutoMapper;
using GameCommerce.Aplicacao.Dtos;
using GameCommerce.Aplicacao.Interfaces;
using GameCommerce.Dominio;
using GameCommerce.Persistencia.Interfaces;

namespace GameCommerce.Aplicacao
{
    public class MarketingTagService : IMarketingTagService
    {
        private readonly IMarketingTagPersist _marketingTagPersist;
        private readonly IMapper _mapper;

        public MarketingTagService(IMarketingTagPersist marketingTagPersist, IMapper mapper)
        {
            _marketingTagPersist = marketingTagPersist;
            _mapper = mapper;
        }

        public async Task<MarketingTagDto> AddAsync(MarketingTagDto model)
        {
            try
            {
                var marketingTag = _mapper.Map<MarketingTag>(model);
                _marketingTagPersist.Add(marketingTag);

                if (await _marketingTagPersist.SaveChangeAsync())
                {
                    var retorno = await _marketingTagPersist.GetByIdAsync(marketingTag.Id);
                    return _mapper.Map<MarketingTagDto>(retorno);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<MarketingTagDto> UpdateAsync(MarketingTagDto model)
        {
            try
            {
                var marketingTag = await _marketingTagPersist.GetByIdAsync(model.Id);
                if (marketingTag == null) return null;

                _mapper.Map(model, marketingTag);
                _marketingTagPersist.Update(marketingTag);

                if (await _marketingTagPersist.SaveChangeAsync())
                {
                    var retorno = await _marketingTagPersist.GetByIdAsync(marketingTag.Id);
                    return _mapper.Map<MarketingTagDto>(retorno);
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
                var marketingTag = await _marketingTagPersist.GetByIdAsync(id);
                if (marketingTag == null) return false;

                marketingTag.Ativo = false;
                _marketingTagPersist.Update(marketingTag);

                return await _marketingTagPersist.SaveChangeAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<MarketingTagDto> GetByIdAsync(int id)
        {
            try
            {
                var marketingTag = await _marketingTagPersist.GetByIdAsync(id);
                if (marketingTag == null) return null;

                return _mapper.Map<MarketingTagDto>(marketingTag);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<MarketingTagDto[]> GetAllAsync(bool apenasAtivos = true)
        {
            try
            {
                var marketingTags = await _marketingTagPersist.GetAllAsync(apenasAtivos);
                if (marketingTags == null) return null;

                return _mapper.Map<MarketingTagDto[]>(marketingTags);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<MarketingTagDto[]> GetBySiteInfoIdAsync(int siteInfoId, bool apenasAtivos = true)
        {
            try
            {
                var marketingTags = await _marketingTagPersist.GetBySiteInfoIdAsync(siteInfoId, apenasAtivos);
                if (marketingTags == null) return null;

                return _mapper.Map<MarketingTagDto[]>(marketingTags);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<MarketingTagDto[]> GetByTipoAsync(string tipo, int siteInfoId, bool apenasAtivos = true)
        {
            try
            {
                var marketingTags = await _marketingTagPersist.GetByTipoAsync(tipo, siteInfoId, apenasAtivos);
                if (marketingTags == null) return null;

                return _mapper.Map<MarketingTagDto[]>(marketingTags);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao buscar marketing tags por tipo: {ex.Message}");
            }
        }

        public async Task<MarketingTagDto[]> GetByIdentificadorAsync(string identificador, int siteInfoId, bool apenasAtivos = true)
        {
            try
            {
                var marketingTags = await _marketingTagPersist.GetByIdentificadorAsync(identificador, siteInfoId, apenasAtivos);
                if (marketingTags == null) return null;

                return _mapper.Map<MarketingTagDto[]>(marketingTags);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao buscar marketing tags por identificador: {ex.Message}");
            }
        }
    }
}