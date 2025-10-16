using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GameCommerce.Dominio;

namespace GameCommerce.Persistencia.Mapeamentos
{
    public class CupomMap : IEntityTypeConfiguration<Cupom>
    {
        public void Configure(EntityTypeBuilder<Cupom> builder)
        {
            // Nome Tabela
            builder.ToTable("Cupons");

            // Chave Primaria
            builder.HasKey(x => x.Id);

            // Propriedades
            builder.Property(x => x.Codigo)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(x => x.ValorDesconto)
                   .HasPrecision(10, 2);

            builder.Property(x => x.TipoDesconto)
                   .IsRequired()
                   .HasConversion<string>()
                   .HasMaxLength(20);

            builder.Property(x => x.MensagemErro)
                   .HasMaxLength(500);

            builder.Property(x => x.Ativo)
                   .IsRequired();

            // Relação com SiteInfo
            builder.HasOne(x => x.SiteInfo)
                   .WithMany(x => x.Cupons)
                   .HasForeignKey(x => x.SiteInfoId)
                   .IsRequired();

            // Index para busca rápida por código
            //builder.HasIndex(x => new { x.SiteInfoId, x.Codigo })
            //       .IsUnique();
        }
    }
}