using GameCommerce.Aplicacao.Dtos;
using GameCommerce.Aplicacao.Helpers;

namespace GameCommerce.Aplicacao.Interfaces
{
    public interface ISiteInfoService
    {
        Task<SiteInfoDto> AddAsync(SiteInfoDto model);
        Task<SiteInfoDto> UpdateAsync(SiteInfoDto model);
        Task<bool> DeleteAsync(int id, bool realDelete = false);

        Task<SiteInfoDto> GetByIdAsync(int id);
        Task<SiteInfoDto[]> GetAllAsync(bool apenasAtivos = true);
        Task<SiteInfoDto[]> GetByTermAsync(string termo, bool apenasAtivos = true);
        Task<SiteInfoDto> GetByDominioAsync(string dominio, bool apenasAtivos = true);


        //Metodos Qantitativos
        Task<int> GetCountAsync(bool apenasAtivos = true);
        Task<PagedResponse<SiteConsolidadoDto>> GetAllConsolidadoAsync(int page = 1, int pageSize = 10, string? search = null, bool apenasAtivos = true);
    }
}