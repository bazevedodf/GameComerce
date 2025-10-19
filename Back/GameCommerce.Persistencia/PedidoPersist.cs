using GameCommerce.Dominio;
using GameCommerce.Persistencia.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace GameCommerce.Persistencia
{
    public class PedidoPersist : GeralPersist, IPedidoPersist
    {
        private readonly AppDbContext _context;

        public PedidoPersist(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Pedido> GetByIdAsync(int id, bool includeItens = true)
        {
            IQueryable<Pedido> query = _context.Pedidos.Include(x => x.TransacaoPagamento)
                                                       .Include(x => x.Cupom)
                                                       .Where(p => p.Id == id);

            if (includeItens)
                query = query.Include(p => p.Itens)
                             .ThenInclude(x => x.Produto);

            return await query.AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<Pedido[]> GetAllAsync(bool includeItens = true)
        {
            IQueryable<Pedido> query = _context.Pedidos.Include(x => x.TransacaoPagamento)
                                                       .Include(x => x.Cupom);

            if (includeItens)
                query = query.Include(p => p.Itens);

            return await query.AsNoTracking().ToArrayAsync();
        }

        public async Task<Pedido[]> GetByTermAsync(string termo, int? siteInfoId = null)
        {
            // Limpa o termo de busca (remove formatação)
            var termoLimpado = LimparTelefone(termo);

            IQueryable<Pedido> query = _context.Pedidos
                .Where(s => s.Email.ToLower()
                                   .Contains(termo.ToLower()) ||
                            s.Telefone.Replace("(", "")
                                      .Replace(")", "")
                                      .Replace("-", "")
                                      .Replace(" ", "")
                                      .Replace("+", "")
                                      .Contains(termoLimpado));
            if (siteInfoId.HasValue)
            {
                query = query.Where(x => x.SiteInfoId == siteInfoId);
            }

            return await query
                .OrderBy(s => s.Email)
                .ThenBy(s => s.Telefone)
                .Take(5)
                .AsNoTracking()
                .ToArrayAsync();
        }

        public async Task<Pedido[]> GetAllBySiteInfoIdAsync(int siteInfoId, bool includeItens = true)
        {
            IQueryable<Pedido> query = _context.Pedidos.Include(x => x.TransacaoPagamento)
                                                       .Include(x => x.Cupom)
                                                       .Where(x => x.SiteInfoId == siteInfoId);

            if (includeItens)
                query = query.Include(p => p.Itens);

            return await query.AsNoTracking().ToArrayAsync();
        }

        public async Task<Pedido> GetByTransactionIdAsync(string transactionId, bool includeItens = true)
        {
            IQueryable<Pedido> query = _context.Pedidos.Include(x => x.TransacaoPagamento)
                                                       .Include(x => x.Cupom)
                                                       .Where(p => p.TransacaoPagamento.TransactionId == transactionId);


            if (includeItens)
                query = query.Include(p => p.Itens);

            return await query.AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<Pedido[]> GetByStatusAsync(string status, bool includeItens = true)
        {
            IQueryable<Pedido> query = _context.Pedidos.Include(x => x.TransacaoPagamento)
                                                       .Include(x => x.Cupom)
                                                       .Where(p => p.Status.ToLower() == status.ToLower());

            if (includeItens)
                query = query.Include(p => p.Itens);

            return await query.AsNoTracking().ToArrayAsync();
        }


        //Metodos Paginados
        public async Task<Pedido[]> GetPaginatedBySiteInfoIdAsync(int page = 1, int pageSize = 10, int? siteInfoId = null, bool includeItens = true)
        {
            IQueryable<Pedido> query = _context.Pedidos.Include(x => x.TransacaoPagamento)
                                                       .Include(x => x.Cupom);
                                                       
            if(siteInfoId.HasValue)
                query = query.Where(x => x.SiteInfoId == siteInfoId);

            if (includeItens)
                query = query.Include(p => p.Itens)
                             .ThenInclude(x => x.Produto);

            return await query
                .OrderBy(s => s.DataCriacao) // Importante para paginação consistente
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToArrayAsync();
        }


        //Metodos quantitativos
        public async Task<int> GetCountAsync(int? siteInfoId = null)
        {
            IQueryable<Pedido> query = _context.Pedidos;

            // Filtro por site (se informado)
            if (siteInfoId.HasValue)
                query = query.Where(p => p.SiteInfoId == siteInfoId.Value);

            return await query.CountAsync();
        }

        public async Task<int> GetCountPagosAsync(int? siteInfoId = null)
        {
            IQueryable<Pedido> query = _context.Pedidos
                .Include(p => p.TransacaoPagamento)
                .Where(p => p.TransacaoPagamento != null &&
                            p.TransacaoPagamento.Status.ToLower() == "paid");

            // Filtro por site (se informado)
            if (siteInfoId.HasValue)
                query = query.Where(p => p.SiteInfoId == siteInfoId.Value);

            return await query.CountAsync();
        }

        public static string LimparTelefone(string telefone)
        {
            if (string.IsNullOrWhiteSpace(telefone))
                return string.Empty;

            // Remove tudo que não é número
            return Regex.Replace(telefone, @"[^\d]", "");
        }
    }
}