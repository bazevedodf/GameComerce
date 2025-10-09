using Microsoft.EntityFrameworkCore;
using GameCommerce.Dominio;
using GameCommerce.Persistencia.Interfaces;

namespace GameCommerce.Persistencia
{
    public class TransacaoPagamentoPersist : GeralPersist, ITransacaoPagamentoPersist
    {
        private readonly AppDbContext _context;

        public TransacaoPagamentoPersist(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<TransacaoPagamento> GetByIdAsync(int id)
        {
            return await _context.TransacoesPagamento
                .Include(t => t.Pedido)
                .Where(t => t.Id == id)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }

        public async Task<TransacaoPagamento> GetByTransactionIdAsync(string transactionId)
        {
            return await _context.TransacoesPagamento
                .Include(t => t.Pedido)
                .Where(t => t.TransactionId == transactionId)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }

        public async Task<TransacaoPagamento> GetByPedidoIdAsync(int pedidoId)
        {
            return await _context.TransacoesPagamento
                .Include(t => t.Pedido)
                .Where(t => t.PedidoId == pedidoId)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }

        public async Task<TransacaoPagamento[]> GetAllAsync()
        {
            return await _context.TransacoesPagamento
                .Include(t => t.Pedido)
                .AsNoTracking()
                .ToArrayAsync();
        }

        public async Task<TransacaoPagamento[]> GetByStatusAsync(string status)
        {
            return await _context.TransacoesPagamento
                .Include(t => t.Pedido)
                .Where(t => t.Status == status)
                .AsNoTracking()
                .ToArrayAsync();
        }
    }
}