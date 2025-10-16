using GameCommerce.Aplicacao.Dtos;

namespace GameCommerce.Aplicacao.Interfaces
{
    public interface IMarketingTagService
    {
        Task<MarketingTagDto> AddAsync(MarketingTagDto model);
        Task<MarketingTagDto> UpdateAsync(MarketingTagDto model);
        Task<bool> DeleteAsync(int id);

        Task<MarketingTagDto> GetByIdAsync(int id);
        Task<MarketingTagDto[]> GetAllAsync(bool apenasAtivos = true);
        Task<MarketingTagDto[]> GetBySiteInfoIdAsync(int siteInfoId, bool apenasAtivos = true);
        Task<MarketingTagDto[]> GetByTipoAsync(string tipo, int siteInfoId, bool apenasAtivos = true);
        Task<MarketingTagDto[]> GetByIdentificadorAsync(string identificador, int siteInfoId, bool apenasAtivos = true);
    }

}