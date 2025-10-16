using Microsoft.EntityFrameworkCore;
using GameCommerce.Dominio;
using GameCommerce.Persistencia.Interfaces;

namespace GameCommerce.Persistencia
{
    public class CupomPersist : GeralPersist, ICupomPersist
    {
        private readonly AppDbContext _context;

        public CupomPersist(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Cupom> GetByIdAsync(int id)
        {
            return await _context.Cupons.Where(c => c.Id == id)
                        .AsNoTracking()
                        .FirstOrDefaultAsync();
        }

        public async Task<Cupom[]> GetBySiteIdAsync(int siteId)
        {
            return await _context.Cupons
                .Where(c => c.SiteInfoId == siteId && c.Ativo && c.Valido)
                .AsNoTracking()
                .ToArrayAsync();
        }

        public async Task<Cupom> GetByCodigoAsync(string codigo)
        {
            return await _context.Cupons
                .Where(c => c.Codigo == codigo)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }

        public async Task<Cupom[]> GetAllAsync(bool apenasAtivos = true)
        {
            IQueryable<Cupom> query = _context.Cupons;

            if (apenasAtivos)
                query = query.Where(c => c.Ativo);

            return await query.AsNoTracking().ToArrayAsync();
        }

        public async Task<Cupom> ValidarCupomAsync(string codigo)
        {
            return await _context.Cupons
                .Where(c => c.Codigo == codigo && c.Ativo && c.Valido)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }
    }
}