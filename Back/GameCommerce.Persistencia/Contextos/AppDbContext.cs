using Microsoft.EntityFrameworkCore;
using GameCommerce.Dominio;
using GameCommerce.Persistencia.Mapeamentos;

namespace GameCommerce.Persistencia
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // DbSets para cada entidade
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Subcategoria> Subcategorias { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Cupom> Cupons { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<ItemPedido> ItensPedido { get; set; }
        public DbSet<SiteInfo> SiteInfos { get; set; }
        public DbSet<MarketingTag> MarketingTags { get; set; }
        
        public DbSet<TransacaoPagamento> TransacoesPagamento { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Aplicar todos os mapeamentos
            modelBuilder.ApplyConfiguration(new CategoriaMap());
            modelBuilder.ApplyConfiguration(new SubcategoriaMap());
            modelBuilder.ApplyConfiguration(new ProdutoMap());
            modelBuilder.ApplyConfiguration(new CupomMap());
            modelBuilder.ApplyConfiguration(new PedidoMap());
            modelBuilder.ApplyConfiguration(new ItemPedidoMap());
            modelBuilder.ApplyConfiguration(new SiteInfoMap());
            modelBuilder.ApplyConfiguration(new MarketingTagMap());
            modelBuilder.ApplyConfiguration(new TransacaoPagamentoMap());
        }
    }
}