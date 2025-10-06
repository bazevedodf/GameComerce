using GameCommerce.Aplicacao.Dtos;

namespace GameCommerce.Aplicacao.Interfaces
{
    public interface ISiteInfoService
    {
        Task<SiteInfoDto> AddAsync(SiteInfoDto model);
        Task<SiteInfoDto> UpdateAsync(SiteInfoDto model);
        Task<bool> DeleteAsync(int id);
        Task<SiteInfoDto> GetByIdAsync(int id);
        Task<SiteInfoDto> GetByDominioAsync(string dominio, bool apenasAtivos = true);
        Task<SiteInfoDto[]> GetAllAsync(bool apenasAtivos = true);
    }
}