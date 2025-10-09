using GameCommerce.Dominio;

namespace GameCommerce.Aplicacao.Dtos
{
    public class ItemPedidoDto
    {
        public int Id { get; set; }
        public int? PedidoId { get; set; }
        public Pedido? Pedido { get; set; }
        public int ProdutoId { get; set; }
        public ProdutoDto? Produto { get; set; }
        public int Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }
        public decimal Subtotal { get; set; }
    }
}