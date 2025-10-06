using GameCommerce.Dominio;

namespace GameCommerce.Persistencia.Interfaces
{
    public interface ITransacaoPagamentoPersist : IGeralPersist
    {
        Task<TransacaoPagamento> GetByIdAsync(int id);
        Task<TransacaoPagamento> GetByTransactionIdAsync(string transactionId);
        Task<TransacaoPagamento> GetByPedidoIdAsync(int pedidoId);
        Task<TransacaoPagamento[]> GetAllAsync();
        Task<TransacaoPagamento[]> GetByStatusAsync(string status);
    }
}