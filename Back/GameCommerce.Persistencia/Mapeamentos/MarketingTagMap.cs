﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GameCommerce.Dominio;

namespace GameCommerce.Persistencia.Mapeamentos
{
    public class MarketingTagMap : IEntityTypeConfiguration<MarketingTag>
    {
        public void Configure(EntityTypeBuilder<MarketingTag> builder)
        {
            // Nome Tabela
            builder.ToTable("MarketingTags");

            // Chave Primária
            builder.HasKey(x => x.Id);

            // Propriedades
            builder.Property(x => x.Tipo)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(x => x.TagId)
                   .IsRequired();

            builder.Property(x => x.Identificador)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(x => x.Nome)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(x => x.Ativo);

            builder.Property(x => x.SiteInfoId)
                   .IsRequired();

            //ÍNDICEs
            builder.HasIndex(x => x.Identificador)
                   .IsUnique();

            // Dados iniciais (seeds) - EXEMPLO
            builder.HasData(
                new MarketingTag
                {
                    Id = 1,
                    Tipo = "google-tag-manager",
                    TagId = "GTM-ABCD123",
                    Identificador = "g-roblox",
                    Nome = "GTM Campanha Principal",
                    Ativo = true,
                    SiteInfoId = 1
                },
                new MarketingTag
                {
                    Id = 2,
                    Tipo = "facebook-pixel",
                    TagId = "123456789012345",
                    Identificador = "f-roblox",
                    Nome = "Facebook Pixel Principal",
                    Ativo = true,
                    SiteInfoId = 1
                },
                new MarketingTag
                {
                    Id = 3,
                    Tipo = "tiktok-pixel",
                    TagId = "ABCDEF123456",
                    Identificador = "tk-roblox",
                    Nome = "TikTok Campanha Natal",
                    Ativo = true,
                    SiteInfoId = 1
                }
            );
        }
    }
}