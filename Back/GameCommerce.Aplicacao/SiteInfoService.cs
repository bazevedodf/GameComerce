using AutoMapper;
using GameCommerce.Aplicacao.Dtos;
using GameCommerce.Aplicacao.Interfaces;
using GameCommerce.Dominio;

namespace GameCommerce.Aplicacao
{
    public class SiteInfoService : ISiteInfoService
    {
        private readonly ISiteInfoPersist _siteInfoPersist;
        private readonly IMapper _mapper;

        public SiteInfoService(ISiteInfoPersist siteInfoPersist, IMapper mapper)
        {
            _siteInfoPersist = siteInfoPersist;
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

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var siteInfo = await _siteInfoPersist.GetByIdAsync(id);
                if (siteInfo == null) return false;

                siteInfo.Ativo = false;
                _siteInfoPersist.Update(siteInfo);

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
    }
}