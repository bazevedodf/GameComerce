using Microsoft.EntityFrameworkCore;
using GameCommerce.Dominio;
using GameCommerce.Persistencia.Interfaces;
using GameCommerce.Dominio.Enuns;

namespace GameCommerce.Persistencia
{
    public class PedidoPersist : GeralPersist, IPedidoPersist
    {
        private readonly AppDbContext _context;

        public PedidoPersist(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Pedido> GetByIdAsync(int id, bool includeItens = true, bool includeCupom = true)
        {
            IQueryable<Pedido> query = _context.Pedidos.Where(p => p.Id == id);

            if (includeItens)
                query = query.Include(p => p.Itens);

            if (includeCupom)
                query = query.Include(p => p.Cupom);

            return await query.AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<Pedido[]> GetAllAsync(bool includeItens = true, bool includeCupom = true)
        {
            IQueryable<Pedido> query = _context.Pedidos;

            if (includeItens)
                query = query.Include(p => p.Itens);

            if (includeCupom)
                query = query.Include(p => p.Cupom);

            return await query.AsNoTracking().ToArrayAsync();
        }

        public async Task<Pedido> GetByTransactionIdAsync(string transactionId, bool includeItens = true)
        {
            IQueryable<Pedido> query = _context.Pedidos.Where(p => p.TransacaoPagamento.TransactionId == transactionId);

            if (includeItens)
                query = query.Include(p => p.Itens);

            return await query.AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<Pedido[]> GetByStatusAsync(StatusPedido status, bool includeItens = true)
        {
            IQueryable<Pedido> query = _context.Pedidos.Where(p => p.Status == status);

            if (includeItens)
                query = query.Include(p => p.Itens);

            return await query.AsNoTracking().ToArrayAsync();
        }
    }
}