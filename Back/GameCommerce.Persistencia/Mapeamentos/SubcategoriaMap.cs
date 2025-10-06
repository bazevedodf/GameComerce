using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GameCommerce.Dominio;

namespace GameCommerce.Persistencia.Mapeamentos
{
    public class SubcategoriaMap : IEntityTypeConfiguration<Subcategoria>
    {
        public void Configure(EntityTypeBuilder<Subcategoria> builder)
        {
            // Nome Tabela
            builder.ToTable("Subcategorias");

            // Chave Primaria
            builder.HasKey(x => x.Id);

            // Propriedades
            builder.Property(x => x.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(x => x.Slug)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(x => x.Ativo)
                   .IsRequired();

            builder.HasData(
                new Subcategoria { 
                    Id = 1, 
                    Name = "CONTAS FORTNITE", 
                    Slug = "contas-fortnite", 
                    CategoriaId = 1 
                },
                new Subcategoria { 
                    Id = 2, 
                    Name = "BUNDLES FORTNITE", 
                    Slug = "bundles-fortnite", 
                    CategoriaId = 1 
                },
                new Subcategoria { 
                    Id = 3, 
                    Name = "CONTAS FREEFIRE", 
                    Slug = "contas-freefire", 
                    CategoriaId = 2 
                }
            );
        }
    }
}