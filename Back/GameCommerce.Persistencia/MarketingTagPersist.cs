using Microsoft.EntityFrameworkCore;
using GameCommerce.Dominio;
using GameCommerce.Persistencia.Interfaces;

namespace GameCommerce.Persistencia
{
    public class MarketingTagPersist : GeralPersist, IMarketingTagPersist
    {
        private readonly AppDbContext _context;

        public MarketingTagPersist(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<MarketingTag> GetByIdAsync(int id)
        {
            return await _context.MarketingTags
                .Where(m => m.Id == id)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }

        public async Task<MarketingTag[]> GetAllAsync(bool apenasAtivos = true)
        {
            IQueryable<MarketingTag> query = _context.MarketingTags;

            if (apenasAtivos)
                query = query.Where(m => m.Ativo);

            return await query.AsNoTracking().ToArrayAsync();
        }

        public async Task<MarketingTag[]> GetBySiteInfoIdAsync(int siteInfoId, bool apenasAtivos = true)
        {
            IQueryable<MarketingTag> query = _context.MarketingTags
                .Where(m => m.SiteInfoId == siteInfoId);

            if (apenasAtivos)
                query = query.Where(m => m.Ativo);

            return await query.AsNoTracking().ToArrayAsync();
        }

        public async Task<MarketingTag[]> GetByTipoAsync(string tipo, int siteInfoId, bool apenasAtivos = true)
        {
            IQueryable<MarketingTag> query = _context.MarketingTags
                .Where(m => m.Tipo.ToLower() == tipo.ToLower() && m.SiteInfoId == siteInfoId);

            if (apenasAtivos)
                query = query.Where(m => m.Ativo);

            return await query.AsNoTracking().ToArrayAsync();
        }

        public async Task<MarketingTag[]> GetByIdentificadorAsync(string identificador, int siteInfoId, bool apenasAtivos = true)
        {
            IQueryable<MarketingTag> query = _context.MarketingTags
                .Where(m => m.Identificador.ToLower() == identificador.ToLower() && m.SiteInfoId == siteInfoId);

            if (apenasAtivos)
                query = query.Where(m => m.Ativo);

            return await query.AsNoTracking().ToArrayAsync();
        }
    }
}