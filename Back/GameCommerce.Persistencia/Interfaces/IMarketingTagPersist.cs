using GameCommerce.Dominio;

namespace GameCommerce.Persistencia.Interfaces
{
    public interface IMarketingTagPersist : IGeralPersist
    {
        Task<MarketingTag> GetByIdAsync(int id);
        Task<MarketingTag[]> GetAllAsync(bool apenasAtivos = true);
        Task<MarketingTag[]> GetBySiteInfoIdAsync(int siteInfoId, bool apenasAtivos = true);
        Task<MarketingTag[]> GetByTipoAsync(string tipo, int siteInfoId, bool apenasAtivos = true);
        Task<MarketingTag[]> GetByIdentificadorAsync(string identificador, int siteInfoId, bool apenasAtivos = true);
    }
}