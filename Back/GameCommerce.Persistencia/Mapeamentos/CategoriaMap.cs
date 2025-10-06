using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GameCommerce.Dominio;

namespace GameCommerce.Persistencia.Mapeamentos
{
    public class CategoriaMap : IEntityTypeConfiguration<Categoria>
    {
        public void Configure(EntityTypeBuilder<Categoria> builder)
        {
            // Nome Tabela
            builder.ToTable("Categorias");

            // Chave Primaria
            builder.HasKey(x => x.Id);

            // Propriedades
            builder.Property(x => x.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(x => x.Slug)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(x => x.Descricao)
                   .HasMaxLength(500);

            builder.Property(x => x.Imagem)
                   .HasMaxLength(500);

            builder.Property(x => x.Icon)
                   .HasMaxLength(500);

            builder.Property(x => x.Ativo)
                   .IsRequired();

            // Relacionamento 1:N com Subcategorias
            builder.HasMany(x => x.Subcategorias)
                   .WithOne(x => x.Categoria)
                   .HasForeignKey(x => x.CategoriaId);

            // Relação com SiteInfo
            builder.HasOne(x => x.SiteInfo)
                   .WithMany(x => x.Categorias)
                   .HasForeignKey(x => x.SiteInfoId)
                   .IsRequired();

            builder.HasData(
                // CATEGORIAS
                new Categoria { 
                    Id = 1, 
                    Name = "FORTNITE", 
                    Slug = "fortnite", 
                    Descricao = "Explore, crie e brilhe no Fortnite com estilo!", 
                    Imagem = "/uploads/categorias/categoria-fortnite.png", 
                    Icon = "/uploads/categorias/fortnite-ico.png", 
                    SiteInfoId = 1 
                },
                new Categoria { 
                    Id = 2, 
                    Name = "FREE FIRE", 
                    Slug = "free-fire", 
                    Descricao = "Ação rápida e intensa em batalhas épicas.", 
                    Imagem = "/uploads/categorias/categoria-free-fire.webp", 
                    Icon = "/uploads/categorias/freefire-ico.png", 
                    SiteInfoId = 1 
                },
                new Categoria { 
                    Id = 3, 
                    Name = "VALORANT", 
                    Slug = "valorant", 
                    Descricao = "Ação tática com estilo e precisão.", 
                    Imagem = "/uploads/categorias/categoria-valorant.jpg", 
                    Icon = "/uploads/categorias/valorant-ico.png", 
                    SiteInfoId = 1 
                },
                new Categoria { 
                    Id = 4, 
                    Name = "LEAGUE OF LEGENDS", 
                    Slug = "league-of-legends", 
                    Descricao = "Batalhas estratégicas em um mundo de fantasia.", 
                    Imagem = "/uploads/categorias/categoria-legue-of-legend.png", 
                    Icon = "/uploads/categorias/legue-of-legends-ico.png", 
                    SiteInfoId = 1 
                },
                new Categoria { 
                    Id = 5, 
                    Name = "ROBLOX", 
                    Slug = "roblox", 
                    Descricao = "Explore, crie e brilhe no Roblox com estilo!", 
                    Imagem = "/uploads/categorias/categoria-roblox.webp", 
                    Icon = "/uploads/categorias/robux-ico.png", 
                    SiteInfoId = 1 
                },
                new Categoria { 
                    Id = 6, 
                    Name = "BRAWL STARS", 
                    Slug = "brawl-stars", 
                    Descricao = "Lutas rápidas e divertidas com amigos.", 
                    Imagem = "/uploads/categorias/categoria-brawl-stars.webp", 
                    Icon = "/uploads/categorias/brawstars-ico.png", 
                    SiteInfoId = 1 
                }
            );
        }
    }
}