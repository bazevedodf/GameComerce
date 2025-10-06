using GameCommerce.Aplicacao.Dtos;

namespace GameCommerce.Aplicacao.Interfaces
{
    public interface ICategoriaService
    {
        Task<CategoriaDto> AddAsync(CategoriaDto model);
        Task<CategoriaDto> UpdateAsync(CategoriaDto model);
        Task<bool> DeleteAsync(int id);
        Task<CategoriaDto[]> BuscarAsync(string termo);
        Task<CategoriaDto[]> GetAllAsync(bool includeSubcategorias = true);
        Task<CategoriaDto> GetByIdAsync(int id, bool includeSubcategorias = true);
        Task<CategoriaDto> GetBySlugAsync(string slug, bool includeSubcategorias = true);
    }
}