using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GameCommerce.Dominio;

namespace GameCommerce.Persistencia.Mapeamentos
{
    public class SiteInfoMap : IEntityTypeConfiguration<SiteInfo>
    {
        public void Configure(EntityTypeBuilder<SiteInfo> builder)
        {
            // Nome Tabela
            builder.ToTable("SiteInfo");

            // Chave Primaria
            builder.HasKey(x => x.Id);

            // Propriedades
            builder.Property(x => x.Nome)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(x => x.Dominio)
                   .HasMaxLength(100);

            builder.Property(x => x.LogoUrl)
                   .HasMaxLength(500);

            builder.Property(x => x.Cnpj)
                   .HasMaxLength(20);

            builder.Property(x => x.Address)
                   .HasMaxLength(500);

            builder.Property(x => x.Email)
                   .HasMaxLength(255);

            builder.Property(x => x.Instagram)
                   .HasMaxLength(200);

            builder.Property(x => x.Facebook)
                   .HasMaxLength(200);

            builder.Property(x => x.Whatsapp)
                   .HasMaxLength(20);

            builder.Property(x => x.YouTube)
                   .HasMaxLength(200);

            builder.Property(x => x.Ativo)
                   .IsRequired();

            builder.HasData(
                new SiteInfo
                    {
                        Id = 1,
                        Nome = "Game Commerce",
                        Dominio = "localhost:4200",
                        LogoUrl = "/uploads/logo-site.png",
                        Cnpj = "12.345.678/0001-99",
                        Address = "Rua dos Games, 123 - São Paulo, SP",
                        Email = "contato@gamecommerce.com.br",
                        Instagram = "https://instagram.com/gamecommerce",
                        Facebook = "https://facebook.com/gamecommerce",
                        Whatsapp = "(11) 99999-9999"
                    }
            );
        }
    }
}