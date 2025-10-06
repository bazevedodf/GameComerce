using GameCommerce.Dominio;

namespace GameCommerce.Persistencia.Interfaces
{
    public interface IProdutoPersist : IGeralPersist
    {
        Task<Produto[]> GetAllAsync(bool includeCategoria = true);
        Task<Produto> GetByIdAsync(int id, bool includeCategoria = true);
        Task<Produto[]> GetByCategoriaAsync(string categoriaSlug, bool includeCategoria = true);
        Task<Produto[]> GetDestaquesAsync(bool includeCategoria = true);
        Task<Produto[]> BuscarAsync(string termo, bool includeCategoria = true);
        Task<Produto[]> GetByTagAsync(string tag, bool includeCategoria = true);
        Task<Produto[]> GetMaisVendidosPorCategoriaAsync(bool includeCategoria = true);
    }
}