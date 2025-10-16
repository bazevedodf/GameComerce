using GameCommerce.Dominio;

namespace GameCommerce.Persistencia.Interfaces
{
    public interface ICupomPersist : IGeralPersist
    {
        Task<Cupom> GetByIdAsync(int id);
        Task<Cupom[]> GetBySiteIdAsync(int siteId);
        Task<Cupom> GetByCodigoAsync(string codigo);
        Task<Cupom[]> GetAllAsync(bool apenasAtivos = true);
        Task<Cupom> ValidarCupomAsync(string codigo);
    }
}