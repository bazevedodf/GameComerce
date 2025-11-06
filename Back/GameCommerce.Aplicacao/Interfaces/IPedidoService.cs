using GameCommerce.Aplicacao.Dtos;
using GameCommerce.Aplicacao.Helpers;
using GameCommerce.Dominio;

namespace GameCommerce.Aplicacao.Interfaces
{
    public interface IPedidoService
    {
        Task<PedidoDto> AddAsync(PedidoDto model);
        Task<PedidoDto> UpdateAsync(PedidoDto model);
        Task<bool> DeleteAsync(int id);

        Task<PedidoDto> GetByIdAsync(int id, bool includeItens = true);
        Task<PedidoDto[]> GetAllAsync(bool includeItens = true);
        Task<PedidoDto> GetByTransactionIdAsync(string transactionId, bool includeItens = true);
        Task<PedidoDto[]> GetAllBySiteInfoIdAsync(int siteInfoId, bool includeItens = true);
        Task<PedidoDto[]> GetByStatusAsync(string status, bool includeItens = true);

        //Metodos Paginados
        Task<PagedResponse<PedidoDto>> GetPaginatedBySiteInfoIdAsync(int page = 1, int pageSize = 10, int? siteInfoId = null, bool includeItens = true);

        //Metodos Quantitativos
        Task<int> GetCountAsync(int? siteInfoId = null);
        Task<int> GetCountPagosAsync(int? siteInfoId = null);

        // Métodos específicos para PIX - AGORA RETORNAM PedidoResponseDto
        Task<PedidoResponseDto> ProcessarPagamentoPixAsync(PedidoDto pedidoDto);
        Task<bool> ProcessarPagamentoConfirmadoAsync(string transactionId);
        Task<PedidoResponseDto> VerificarStatusPagamentoAsync(string transactionId, bool includeItens = true);
    }
}