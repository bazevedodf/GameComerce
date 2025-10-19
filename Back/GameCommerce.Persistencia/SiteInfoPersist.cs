using GameCommerce.Dominio;
using GameCommerce.Persistencia.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

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
            return await _context.SiteInfos.Where(s => s.Id == id)
                                           .AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<SiteInfo[]> GetAllAsync(bool apenasAtivos = true)
        {
            IQueryable<SiteInfo> query = _context.SiteInfos;

            if (apenasAtivos)
                query = query.Where(s => s.Ativo);

            return await query.AsNoTracking().ToArrayAsync();
        }

        public async Task<int> GetTotalCountAsync(bool apenasAtivos = true)
        {
            IQueryable<SiteInfo> query = _context.SiteInfos;

            if (apenasAtivos)
                query = query.Where(s => s.Ativo);

            return await query.CountAsync();
        }

        public async Task<SiteInfo[]> GetByTermAsync(string termo, bool apenasAtivos = true)
        {

            IQueryable<SiteInfo> query = _context.SiteInfos
                .Where(s => s.Nome.ToLower()
                                   .Contains(termo.ToLower()) || 
                            s.Dominio.ToLower().Contains(termo.ToLower()));

            if (apenasAtivos)
                query = query.Where(s => s.Ativo);

            return await query
                .OrderBy(s => s.Nome)
                .ThenBy(s => s.Whatsapp)
                .Take(5)
                .AsNoTracking()
                .ToArrayAsync();
        }

        public async Task<SiteInfo> GetByDominioAsync(string dominio, bool apenasAtivos = true)
        {
            IQueryable<SiteInfo> query = _context.SiteInfos
                .Where(s => s.Dominio.ToLower() == dominio.ToLower())
                .Include(x => x.MarketingTags);

            if (apenasAtivos)
                query = query.Where(s => s.Ativo);

            return await query.AsNoTracking().FirstOrDefaultAsync();
        }


        //Metodos Quantitativos
        public async Task<int> GetCountAsync(bool apenasAtivos = true)
        {
            IQueryable<SiteInfo> query = _context.SiteInfos;

            if (apenasAtivos)
                query = query.Where(s => s.Ativo);

            return await query.CountAsync();
        }
        public async Task<SiteInfo[]> GetAllConsolidadoAsync(int page = 1, int pageSize = 10, string search = null, bool apenasAtivos = true)
        {
            IQueryable<SiteInfo> query = _context.SiteInfos
                                            .Include(x => x.Cupons)
                                            .Include(x => x.Produtos);

            if (search != null)
            {
                query = query.Where(s => s.Dominio.ToLower().Contains(search.ToLower())
                                      || s.Nome.ToLower().Contains(search.ToLower()));
            }
                               


            if (apenasAtivos)
                query = query.Where(s => s.Ativo);                                

            return await query
                .OrderBy(s => s.Id) // Importante para paginação consistente
                .ThenBy(s => s.Nome)
                .ThenBy(s => s.Dominio)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToArrayAsync();
        }

    }
}