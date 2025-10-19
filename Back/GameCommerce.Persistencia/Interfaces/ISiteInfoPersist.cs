using GameCommerce.Dominio;

namespace GameCommerce.Persistencia.Interfaces
{
    public interface ISiteInfoPersist : IGeralPersist
    {
        Task<SiteInfo> GetByIdAsync(int id);
        Task<SiteInfo[]> GetAllAsync(bool apenasAtivos = true);
        Task<int> GetTotalCountAsync(bool apenasAtivos = true);
        Task<SiteInfo[]> GetByTermAsync(string termo, bool apenasAtivos = true);
        Task<SiteInfo> GetByDominioAsync(string dominio, bool apenasAtivos = true);
        Task<SiteInfo[]> GetAllConsolidadoAsync(int page = 1, int pageSize = 10, string? search = null, bool apenasAtivos = true);

        //Metodos Quantitativos
        Task<int> GetCountAsync(bool apenasAtivos = true);
    }
}