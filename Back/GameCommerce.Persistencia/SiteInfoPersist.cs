using Microsoft.EntityFrameworkCore;
using GameCommerce.Dominio;
using GameCommerce.Persistencia.Interfaces;

namespace GameCommerce.Persistencia
{
    public class SiteInfoPersist : GeralPersist, ISiteInfoPersist
    {
        private readonly AppDbContext _context;

        public SiteInfoPersist(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<SiteInfo> GetByIdAsync(int id)
        {
            return await _context.SiteInfos
                .Where(s => s.Id == id && s.Ativo)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }

        public async Task<SiteInfo[]> GetAllAsync(bool apenasAtivos = true)
        {
            IQueryable<SiteInfo> query = _context.SiteInfos;

            if (apenasAtivos)
                query = query.Where(s => s.Ativo);

            return await query.AsNoTracking().ToArrayAsync();
        }

        public async Task<SiteInfo> GetByDominioAsync(string dominio, bool apenasAtivos = true)
        {
            IQueryable<SiteInfo> query = _context.SiteInfos
                .Where(s => s.Dominio.ToLower() == dominio.ToLower());

            if (apenasAtivos)
                query = query.Where(s => s.Ativo);

            return await query.AsNoTracking().FirstOrDefaultAsync();
        }
    }
}