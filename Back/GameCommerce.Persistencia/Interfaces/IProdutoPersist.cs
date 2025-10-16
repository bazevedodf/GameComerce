using GameCommerce.Dominio;

namespace GameCommerce.Persistencia.Interfaces
{
    public interface IProdutoPersist : IGeralPersist
    {
        Task<Produto[]> GetAllAsync(bool includeCategoria = true);
        Task<Produto> GetByIdAsync(int id, bool includeCategoria = true);
        Task<Produto[]> GetDestaquesAsync(int siteId, bool includeCategoria = true);
        Task<Produto[]> GetByTagAsync(string tag, bool includeCategoria = true);
        Task<Produto[]> GetBySiteIdAsync(int siteId, bool includeCategoria = true);
        Task<Produto[]> BuscarAsync(int siteId, string termo, bool includeCategoria = true);
        Task<Produto[]> GetMaisVendidosPorCategoriaAsync(int siteId, bool includeCategoria = true);
        Task<Produto[]> GetByCategoriaAsync(int siteId, string categoriaSlug, bool includeCategoria = true);
    }
}