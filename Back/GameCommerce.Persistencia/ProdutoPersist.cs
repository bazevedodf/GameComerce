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

        public async Task<Produto> GetByIdAsync(int id, bool includeCategoria = true)
        {
            IQueryable<Produto> query = _context.Produtos.Where(p => p.Id == id && p.Ativo);

            if (includeCategoria)
                query = query.Include(p => p.Categoria);

            return await query.AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<Produto[]> GetByCategoriaAsync(string categoriaSlug, bool includeCategoria = true)
        {
            IQueryable<Produto> query = _context.Produtos
                .Where(p => p.Categoria.Slug == categoriaSlug && p.Ativo);

            if (includeCategoria)
                query = query.Include(p => p.Categoria);

            return await query.AsNoTracking().ToArrayAsync();
        }

        public async Task<Produto[]> GetDestaquesAsync(bool includeCategoria = true)
        {
            IQueryable<Produto> query = _context.Produtos
                .Where(p => p.EmDestaque && p.Ativo);

            if (includeCategoria)
                query = query.Include(p => p.Categoria);

            return await query.AsNoTracking().ToArrayAsync();
        }

        public async Task<Produto[]> BuscarAsync(string termo, bool includeCategoria = true)
        {
            IQueryable<Produto> query = _context.Produtos
                .Where(p => p.Ativo && (
                    p.Nome.Contains(termo) ||
                    p.Descricao.Contains(termo) ||
                    p.Tags.Any(tag => tag.Contains(termo))
                ));

            if (includeCategoria)
                query = query.Include(p => p.Categoria);

            return await query.AsNoTracking().ToArrayAsync();
        }

        public async Task<Produto[]> GetByTagAsync(string tag, bool includeCategoria = true)
        {
            IQueryable<Produto> query = _context.Produtos
                .Where(p => p.Ativo && p.Tags.Any(t => t == tag));

            if (includeCategoria)
                query = query.Include(p => p.Categoria);

            return await query.AsNoTracking().ToArrayAsync();
        }

        public async Task<Produto[]> GetMaisVendidosPorCategoriaAsync(bool includeCategoria = true)
        {
            var categorias = await _context.Categorias
                .Where(c => c.Ativo)
                .Select(c => c.Id)
                .ToArrayAsync();

            var produtosDestaquePorCategoria = new List<Produto>();

            foreach (var categoriaId in categorias)
            {
                // Buscar UM produto ATIVO e EM DESTAQUE da categoria
                IQueryable<Produto> query = _context.Produtos
                    .Where(p => p.Ativo && p.EmDestaque && p.CategoriaId == categoriaId)
                    .OrderByDescending(p => p.Id); // Ordena por melhor avaliação
                    //.ThenByDescending(p => p.Id);        // Depois por ID mais recente

                // Aplicar include se necessário
                if (includeCategoria)
                    query = query.Include(p => p.Categoria);

                var produto = await query.AsNoTracking().FirstOrDefaultAsync();

                if (produto != null)
                {
                    produtosDestaquePorCategoria.Add(produto);
                }
            }

            return produtosDestaquePorCategoria.ToArray();
        }
    }
}