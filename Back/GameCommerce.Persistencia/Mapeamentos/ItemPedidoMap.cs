using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GameCommerce.Dominio;

namespace GameCommerce.Persistencia.Mapeamentos
{
    public class ItemPedidoMap : IEntityTypeConfiguration<ItemPedido>
    {
        public void Configure(EntityTypeBuilder<ItemPedido> builder)
        {
            // Nome Tabela
            builder.ToTable("ItensPedido");

            // Chave Primaria
            builder.HasKey(x => x.Id);

            // Propriedades
            builder.Property(x => x.Quantidade)
                   .IsRequired();

            builder.Property(x => x.PrecoUnitario)
                   .IsRequired()
                   .HasPrecision(10, 2);

            // Relacionamento com Pedido
            builder.HasOne(x => x.Pedido)
                   .WithMany(x => x.Itens)
                   .HasForeignKey(x => x.PedidoId);

            // Relacionamento com Produto
            builder.HasOne(x => x.Produto)
                   .WithMany()
                   .HasForeignKey(x => x.ProdutoId);
        }
    }
}