using GameCommerce.Dominio;

namespace GameCommerce.Persistencia.Interfaces
{
    public interface ISiteInfoPersist : IGeralPersist
    {
        Task<SiteInfo> GetByIdAsync(int id);
        Task<SiteInfo[]> GetAllAsync(bool apenasAtivos = true);
        Task<SiteInfo> GetByDominioAsync(string dominio, bool apenasAtivos = true);
    }
}