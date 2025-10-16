using Microsoft.EntityFrameworkCore;
using GameCommerce.Dominio;
using GameCommerce.Persistencia.Interfaces;

namespace GameCommerce.Persistencia
{
    public class ProdutoPersist : GeralPersist, IProdutoPersist
    {
        private readonly AppDbContext _context;

        public ProdutoPersist(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Produto[]> GetAllAsync(bool includeCategoria = true)
        {
            IQueryable<Produto> query = _context.Produtos.Where(p => p.Ativo);

            if (includeCategoria)
                query = query.Include(p => p.Categoria);

            return await query.AsNoTracking().ToArrayAsync();
        }

        public async Task<Produto[]> GetBySiteIdAsync(int siteId, bool includeCategoria = true)
        {
            IQueryable<Produto> query = _context.Produtos.Where(p => p.SiteInfoId == siteId && p.Ativo);

            if (includeCategoria)
                query = query.Include(p => p.Categoria);

            return await query.AsNoTracking().ToArrayAsync();
        }

        public async Task<Produto> GetByIdAsync(int id, bool includeCategoria = true)
        {
            IQueryable<Produto> query = _context.Produtos.Where(p => p.Id == id);

            if (includeCategoria)
                query = query.Include(p => p.Categoria);

            return await query.AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<Produto[]> GetByCategoriaAsync(int siteId, string categoriaSlug, bool includeCategoria = true)
        {
            IQueryable<Produto> query = _context.Produtos
                .Where(p => p.SiteInfoId == siteId && p.Categoria.Slug == categoriaSlug && p.Ativo);

            if (includeCategoria)
                query = query.Include(p => p.Categoria);

            return await query.AsNoTracking().ToArrayAsync();
        }

        public async Task<Produto[]> GetDestaquesAsync(int siteId, bool includeCategoria = true)
        {
            IQueryable<Produto> query = _context.Produtos
                .Where(p => p.SiteInfoId == siteId && p.EmDestaque && p.Ativo);

            if (includeCategoria)
                query = query.Include(p => p.Categoria);

            return await query.AsNoTracking().ToArrayAsync();
        }

        public async Task<Produto[]> BuscarAsync(int siteId, string termo, bool includeCategoria = true)
        {
            // Primeiro busca os produtos por nome e descrição no banco
            IQueryable<Produto> query = _context.Produtos
                .Where(p => p.SiteInfoId == siteId && p.Ativo && (
                    p.Nome.Contains(termo) ||
                    p.Descricao.Contains(termo)
                ));

            if (includeCategoria)
                query = query.Include(p => p.Categoria);

            // Executa a query no banco
            var produtos = await query.AsNoTracking().ToArrayAsync();

            // Filtra por tags na memória (client evaluation)
            var produtosComTags = produtos.Where(p =>
                p.Tags?.Any(tag => tag.Contains(termo)) == true
            ).ToArray();

            // Combina os resultados
            var todosProdutos = produtos.Union(produtosComTags).Distinct().ToArray();

            return todosProdutos;
        }

        public async Task<Produto[]> GetByTagAsync(string tag, bool includeCategoria = true)
        {
            IQueryable<Produto> query = _context.Produtos
                .Where(p => p.Ativo && p.Tags.Any(t => t == tag));

            if (includeCategoria)
                query = query.Include(p => p.Categoria);

            return await query.AsNoTracking().ToArrayAsync();
        }

        public async Task<Produto[]> GetMaisVendidosPorCategoriaAsync(int siteId, bool includeCategoria = true)
        {
            // Buscar TODOS os produtos ATIVOS e EM DESTAQUE
            IQueryable<Produto> query = _context.Produtos
                    .Where(p => p.SiteInfoId == siteId && p.Ativo && p.EmDestaque)
                    .OrderByDescending(p => p.TotalAvaliacoes);

            // Aplicar include se necessário
            if (includeCategoria)
                query = query.Include(p => p.Categoria);

            return await query.AsNoTracking().ToArrayAsync();
        }
    }
}