using Microsoft.EntityFrameworkCore;
using GameCommerce.Dominio;
using GameCommerce.Persistencia.Interfaces;

namespace GameCommerce.Persistencia
{
    public class CategoriaPersist : GeralPersist, ICategoriaPersist
    {
        private readonly AppDbContext _context;

        public CategoriaPersist(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Categoria[]> BuscarAsync(string termo)
        {
            return await _context.Categorias
                .Where(c => c.Ativo &&
                    (c.Name.Contains(termo) || c.Slug.Contains(termo)))
                .AsNoTracking()
                .ToArrayAsync();
        }

        public async Task<Categoria[]> GetAllAsync(bool includeSubcategorias = true)
        {
            IQueryable<Categoria> query = _context.Categorias.Where(c => c.Ativo);

            if (includeSubcategorias)
                query = query.Include(c => c.Subcategorias.Where(s => s.Ativo));

            return await query.AsNoTracking().ToArrayAsync();
        }

        public async Task<Categoria[]> GetAllBySiteIdAsync(int siteId, bool includeSubcategorias = true)
        {
            IQueryable<Categoria> query = _context.Categorias.Where(c => c.SiteInfoId == siteId && c.Ativo);

            if (includeSubcategorias)
                query = query.Include(c => c.Subcategorias.Where(s => s.Ativo));

            return await query.AsNoTracking().ToArrayAsync();
        }

        public async Task<Categoria> GetByIdAsync(int id, bool includeSubcategorias = true)
        {
            IQueryable<Categoria> query = _context.Categorias.Where(c => c.Id == id && c.Ativo);

            if (includeSubcategorias)
                query = query.Include(c => c.Subcategorias.Where(s => s.Ativo));

            return await query.AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<Categoria> GetBySlugAsync(string slug, bool includeSubcategorias = true)
        {
            IQueryable<Categoria> query = _context.Categorias.Where(c => c.Slug == slug && c.Ativo);

            if (includeSubcategorias)
                query = query.Include(c => c.Subcategorias.Where(s => s.Ativo));

            return await query.AsNoTracking().FirstOrDefaultAsync();
        }

    }
}