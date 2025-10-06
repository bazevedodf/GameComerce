using GameCommerce.Dominio;
using GameCommerce.Dominio.Enuns;

namespace GameCommerce.Persistencia.Interfaces
{
    public interface IPedidoPersist : IGeralPersist
    {
        Task<Pedido> GetByIdAsync(int id, bool includeItens = true, bool includeCupom = true);
        Task<Pedido[]> GetAllAsync(bool includeItens = true, bool includeCupom = true);
        Task<Pedido> GetByTransactionIdAsync(string transactionId, bool includeItens = true);
        Task<Pedido[]> GetByStatusAsync(StatusPedido status, bool includeItens = true);
    }
}