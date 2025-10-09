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

            // RELACIONAMENTO COM PEDIDO
            builder.Property(x => x.PedidoId)
                   .IsRequired();

            // DADOS ENVIADOS
            builder.Property(x => x.Amount).IsRequired();
            builder.Property(x => x.PaymentMethod)
                   .HasMaxLength(20)
                   .IsRequired()
                   .HasDefaultValue("Pix");

            // CUSTOMER
            builder.Property(x => x.CustomerName)
                   .HasMaxLength(255)
                   .IsRequired(false);

            builder.Property(x => x.CustomerEmail)
                   .HasMaxLength(255)
                   .IsRequired(false);

            builder.Property(x => x.CustomerPhone)
                   .HasMaxLength(20)
                   .IsRequired(false);

            builder.Property(x => x.CustomerDocument)
                   .HasMaxLength(20)
                   .IsRequired(false);

            // ADDRESS
            builder.Property(x => x.ZipCode)
                   .HasMaxLength(10)
                   .IsRequired(false);

            builder.Property(x => x.Street)
                   .HasMaxLength(255)
                   .IsRequired(false);

            builder.Property(x => x.Number)
                   .HasMaxLength(10)
                   .IsRequired(false);

            builder.Property(x => x.Neighborhood)
                   .HasMaxLength(100)
                   .IsRequired(false);

            builder.Property(x => x.City)
                   .HasMaxLength(100)
                   .IsRequired(false);

            builder.Property(x => x.State)
                   .HasMaxLength(2)
                   .IsRequired(false);

            builder.Property(x => x.Country)
                   .HasMaxLength(2)
                   .IsRequired()
                   .HasDefaultValue("BR");

            // RESPOSTA GATEWAY
            builder.Property(x => x.TransactionId)
                   .HasMaxLength(100)
                   .IsRequired();

            builder.Property(x => x.Status)
                   .HasMaxLength(20)
                   .IsRequired(false);

            builder.Property(x => x.PixCode)
                   .HasMaxLength(500)
                   .IsRequired(false);

            builder.Property(x => x.PostbackUrl)
                   .HasMaxLength(500)
                   .IsRequired(false);

            builder.Property(x => x.Message)
                   .HasMaxLength(500)
                   .IsRequired(false);

            builder.Property(x => x.Success)
                   .IsRequired();

            // CONTROLE
            builder.Property(x => x.DataCriacao)
                   .IsRequired()
                   .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(x => x.DataAtualizacao)
                   .IsRequired(false);

            // Relacionamento com Pedido (1:1) - CORRIGIDO
            builder.HasOne(x => x.Pedido)
                   .WithOne(x => x.TransacaoPagamento)
                   .HasForeignKey<TransacaoPagamento>(x => x.PedidoId)
                   .OnDelete(DeleteBehavior.Cascade) // Adicionar esta linha
                   .IsRequired();
        }
    }
}