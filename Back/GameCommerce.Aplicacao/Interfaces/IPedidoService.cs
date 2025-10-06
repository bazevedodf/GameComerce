using GameCommerce.Aplicacao.Dtos;

namespace GameCommerce.Aplicacao.Interfaces
{
    public interface IPedidoService
    {
        Task<PedidoDto> AddAsync(PedidoDto model);
        Task<PedidoDto> UpdateAsync(PedidoDto model);
        Task<bool> DeleteAsync(int id);
        Task<PedidoDto> GetByIdAsync(int id);
        Task<PedidoDto[]> GetAllAsync();
        Task<PedidoDto> GetByTransactionIdAsync(string transactionId);
        Task<PedidoDto[]> GetByStatusAsync(string status);

        // Métodos específicos para PIX - AGORA RETORNAM PedidoResponseDto
        Task<PedidoResponseDto> ProcessarPagamentoPixAsync(PedidoDto pedidoDto);
        Task<PedidoResponseDto> VerificarStatusPagamentoAsync(string transactionId);
    }
}