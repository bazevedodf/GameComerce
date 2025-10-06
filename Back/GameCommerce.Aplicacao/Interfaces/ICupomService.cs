using GameCommerce.Aplicacao.Dtos;

namespace GameCommerce.Aplicacao.Interfaces
{
    public interface ICupomService
    {
        Task<CupomDto> AddAsync(CupomDto model);
        Task<CupomDto> UpdateAsync(CupomDto model);
        Task<bool> DeleteAsync(int id);
        Task<CupomDto> GetByIdAsync(int id);
        Task<CupomDto> GetByCodigoAsync(string codigo);
        Task<CupomDto[]> GetAllAsync();
        Task<CupomDto> ValidarCupomAsync(string codigo);
    }
}