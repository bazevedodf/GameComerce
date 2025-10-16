using GameCommerce.Dominio;

namespace GameCommerce.Persistencia.Interfaces
{
    public interface ICategoriaPersist : IGeralPersist
    {
        Task<Categoria[]> BuscarAsync(string termo);
        Task<Categoria[]> GetAllAsync(bool includeSubcategorias = true);
        Task<Categoria> GetByIdAsync(int id, bool includeSubcategorias = true);
        Task<Categoria> GetBySlugAsync(string slug, bool includeSubcategorias = true);
        Task<Categoria[]> GetAllBySiteIdAsync(int siteId, bool includeSubcategorias = true);
    }
}