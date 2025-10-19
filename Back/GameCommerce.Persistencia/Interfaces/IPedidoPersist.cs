using GameCommerce.Dominio;
using GameCommerce.Dominio.Enuns;

namespace GameCommerce.Persistencia.Interfaces
{
    public interface IPedidoPersist : IGeralPersist
    {
        Task<Pedido[]> GetAllAsync(bool includeItens = true);
        Task<Pedido> GetByIdAsync(int id, bool includeItens = true);
        Task<Pedido[]> GetByTermAsync(string termo, int? siteInfoId = null);
        Task<Pedido> GetByTransactionIdAsync(string transactionId, bool includeItens = true);
        Task<Pedido[]> GetAllBySiteInfoIdAsync(int siteInfoId, bool includeItens = true);
        Task<Pedido[]> GetByStatusAsync(string status, bool includeItens = true);

        //Metodos Paginados
        Task<Pedido[]> GetPaginatedBySiteInfoIdAsync(int page = 1, int pageSize = 10, int? siteInfoId = null, bool includeItens = true);

        //Metodos quantitativos
        Task<int> GetCountAsync(int? siteInfoId = null);
        Task<int> GetCountPagosAsync(int? siteInfoId = null);
    }
}