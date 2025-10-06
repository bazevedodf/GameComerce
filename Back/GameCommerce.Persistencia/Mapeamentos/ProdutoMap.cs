using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GameCommerce.Dominio;

namespace GameCommerce.Persistencia.Mapeamentos
{
    public class ProdutoMap : IEntityTypeConfiguration<Produto>
    {
        public void Configure(EntityTypeBuilder<Produto> builder)
        {
            // Nome Tabela
            builder.ToTable("Produtos");

            // Chave Primaria
            builder.HasKey(x => x.Id);

            // Propriedades
            builder.Property(x => x.Nome)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(x => x.Descricao)
                   .IsRequired()
                   .HasMaxLength(1000);

            builder.Property(x => x.Preco)
                   .IsRequired()
                   .HasPrecision(10, 2);

            builder.Property(x => x.PrecoOriginal)
                   .HasPrecision(10, 2);

            builder.Property(x => x.Imagem)
                   .IsRequired()
                   .HasMaxLength(500);

            builder.Property(x => x.Avaliacao)
                   .HasPrecision(3, 1);

            builder.Property(x => x.Ativo)
                   .IsRequired();

            // Relacionamento N:1 com Categoria
            builder.HasOne(x => x.Categoria)
                   .WithMany()
                   .HasForeignKey(x => x.CategoriaId);
            
            // Relação com SiteInfo
            builder.HasOne(x => x.SiteInfo)
                   .WithMany(x => x.Produtos)
                   .HasForeignKey(x => x.SiteInfoId)
                   .IsRequired();

            // Configuração para List<string> Tags
            builder.Property(x => x.Tags)
                   .HasConversion(
                       v => string.Join(',', v),
                       v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList()
                   );
        }
    }
}