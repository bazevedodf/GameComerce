using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GameCommerce.Dominio;

namespace GameCommerce.Persistencia.Mapeamentos
{
    public class TransacaoPagamentoMap : IEntityTypeConfiguration<TransacaoPagamento>
    {
        public void Configure(EntityTypeBuilder<TransacaoPagamento> builder)
        {
            builder.ToTable("TransacoesPagamento");
            builder.HasKey(x => x.Id);

            // DADOS ENVIADOS
            builder.Property(x => x.Amount).IsRequired();
            builder.Property(x => x.PaymentMethod).HasMaxLength(20).IsRequired();

            // CUSTOMER
            builder.Property(x => x.CustomerName).HasMaxLength(255).IsRequired();
            builder.Property(x => x.CustomerEmail).HasMaxLength(255).IsRequired();
            builder.Property(x => x.CustomerPhone).HasMaxLength(20).IsRequired();
            builder.Property(x => x.CustomerDocument).HasMaxLength(20).IsRequired();

            // ADDRESS
            builder.Property(x => x.ZipCode).HasMaxLength(10).IsRequired();
            builder.Property(x => x.Street).HasMaxLength(255).IsRequired();
            builder.Property(x => x.Number).HasMaxLength(10).IsRequired();
            builder.Property(x => x.Neighborhood).HasMaxLength(100).IsRequired();
            builder.Property(x => x.City).HasMaxLength(100).IsRequired();
            builder.Property(x => x.State).HasMaxLength(2).IsRequired();
            builder.Property(x => x.Country).HasMaxLength(2).IsRequired();

            // RESPOSTA GATEWAY
            builder.Property(x => x.TransactionId).HasMaxLength(100);
            builder.Property(x => x.GatewayStatus).HasMaxLength(20);
            builder.Property(x => x.PixCode).HasMaxLength(500);
            builder.Property(x => x.GatewayCustomerId).HasMaxLength(100);
            builder.Property(x => x.PostbackUrl).HasMaxLength(500);
            builder.Property(x => x.GatewayMessage).HasMaxLength(500);
            builder.Property(x => x.GatewaySuccess);

            // CONTROLE
            builder.Property(x => x.DataCriacao).IsRequired();
            builder.Property(x => x.DataAtualizacao);

            // Relacionamento com Pedido (1:1)
            builder.HasOne(x => x.Pedido)
                   .WithOne(x => x.TransacaoPagamento)
                   .HasForeignKey<TransacaoPagamento>(x => x.PedidoId)
                   .IsRequired();
        }
    }
}