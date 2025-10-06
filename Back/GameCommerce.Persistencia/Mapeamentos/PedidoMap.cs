using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GameCommerce.Dominio;

namespace GameCommerce.Persistencia.Mapeamentos
{
    public class PedidoMap : IEntityTypeConfiguration<Pedido>
    {
        public void Configure(EntityTypeBuilder<Pedido> builder)
        {
            // Nome Tabela
            builder.ToTable("Pedidos");

            // Chave Primaria
            builder.HasKey(x => x.Id);

            // Propriedades
            builder.Property(x => x.Email)
                   .IsRequired()
                   .HasMaxLength(255);

            builder.Property(x => x.Telefone)
                   .HasMaxLength(20);

            builder.Property(x => x.Total)
                   .IsRequired()
                   .HasPrecision(10, 2);

            builder.Property(x => x.Frete)
                   .IsRequired()
                   .HasPrecision(10, 2);

            builder.Property(x => x.MeioPagamento)
                   .IsRequired()
                   .HasConversion<string>()
                   .HasMaxLength(20);

            builder.Property(x => x.DescontoAplicado)
                   .HasPrecision(10, 2);

            builder.Property(x => x.Status)
                   .IsRequired()
                   .HasConversion<string>()
                   .HasMaxLength(20);

            builder.Property(x => x.DataCriacao)
                   .IsRequired();

            // Relação com SiteInfo
            builder.HasOne(x => x.SiteInfo)
                   .WithMany(x => x.Pedidos)
                   .HasForeignKey(x => x.SiteInfoId)
                   .IsRequired();

            // RELAÇÃO COM CUPOM (CORRIGIDA)
            builder.HasOne(x => x.Cupom)
                   .WithMany(x => x.Pedidos)
                   .HasForeignKey(x => x.CupomId)
                   .IsRequired(false)
                   .OnDelete(DeleteBehavior.SetNull);

            // Relacionamento 1:N com ItensPedido
            builder.HasMany(x => x.Itens)
                   .WithOne(x => x.Pedido)
                   .HasForeignKey(x => x.PedidoId);
        }
    }
}