namespace GameCommerce.Aplicacao.Dtos
{
    public class PedidoDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public decimal Total { get; set; }
        public decimal Frete { get; set; }
        public string Status { get; set; } // "Pendente", "Pago", "Expirado", "Cancelado"
        public decimal? DescontoAplicado { get; set; }
        public DateTime DataCriacao { get; set; }
        public string MeioPagamento { get; set; } // "Pix", "CartaoCredito", "Boleto"

        // Relacionamentos
        public int? CupomId { get; set; }
        public CupomDto? Cupom { get; set; }
        public List<ItemPedidoDto>? Itens { get; set; }
        public TransacaoPagamentoDto? TransacaoPagamento { get; set; }
    }
}