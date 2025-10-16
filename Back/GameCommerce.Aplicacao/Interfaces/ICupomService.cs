using GameCommerce.Aplicacao.Dtos;

namespace GameCommerce.Aplicacao.Interfaces
{
    public interface ICupomService
    {
        Task<CupomDto> AddAsync(CupomDto model);
        Task<CupomDto> UpdateAsync(CupomDto model);
        Task<bool> DeleteAsync(int id);

        Task<CupomDto> GetByIdAsync(int id);
        Task<CupomDto[]> GetBySiteIdAsync(int siteId);
        Task<CupomDto> GetByCodigoAsync(string codigo);
        Task<CupomDto> ValidarCupomAsync(string codigo);
        Task<CupomDto[]> GetAllAsync(bool apenasAtivos = true);
        
        Task<int> ClonarCuponsAsync(int siteOrigemId, int siteDestinoId);
    }
}