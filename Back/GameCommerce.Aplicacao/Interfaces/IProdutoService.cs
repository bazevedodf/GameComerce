using GameCommerce.Aplicacao.Dtos;

namespace GameCommerce.Aplicacao.Interfaces
{
    public interface IProdutoService
    {
        Task<ProdutoDto> AddAsync(ProdutoNewDto model);
        Task<ProdutoDto> UpdateAsync(ProdutoDto model);
        Task<bool> DeleteAsync(int id);

        Task<ProdutoDto[]> GetAllAsync(bool includeCategoria = true);
        Task<ProdutoDto[]> GetDestaquesAsync(int siteId, bool includeCategoria = true);
        Task<ProdutoDto> GetByIdAsync(int id, bool includeCategoria = true);
        Task<ProdutoDto[]> GetByTagAsync(string tag, bool includeCategoria = true);
        Task<ProdutoDto[]> GetAllBySiteIdAsync(int siteId, bool includeCategoria = true);
        Task<ProdutoDto[]> BuscarAsync(int siteId, string termo, bool includeCategoria = true);
        Task<ProdutoDto[]> GetMaisVendidosPorCategoriaAsync(int siteId, bool includeCategoria = true);
        Task<ProdutoDto[]> GetByCategoriaAsync(int siteId, string categoriaSlug, bool includeCategoria = true);


        Task<int> ClonarProdutosAsync(int siteOrigemId, int siteDestinoId);
    }
}