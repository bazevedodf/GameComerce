using GameCommerce.Aplicacao.Dtos;

namespace GameCommerce.Aplicacao.Interfaces
{
    public interface IProdutoService
    {
        Task<ProdutoDto> AddAsync(ProdutoDto model);
        Task<ProdutoDto> UpdateAsync(ProdutoDto model);
        Task<bool> DeleteAsync(int id);
        Task<ProdutoDto[]> GetAllAsync(bool includeCategoria = true);
        Task<ProdutoDto[]> GetDestaquesAsync(bool includeCategoria = true);
        Task<ProdutoDto> GetByIdAsync(int id, bool includeCategoria = true);
        Task<ProdutoDto[]> BuscarAsync(string termo, bool includeCategoria = true);
        Task<ProdutoDto[]> GetByTagAsync(string tag, bool includeCategoria = true);
        Task<ProdutoDto[]> GetMaisVendidosPorCategoriaAsync(bool includeCategoria = true);
        Task<ProdutoDto[]> GetByCategoriaAsync(string categoriaSlug, bool includeCategoria = true);
    }
}