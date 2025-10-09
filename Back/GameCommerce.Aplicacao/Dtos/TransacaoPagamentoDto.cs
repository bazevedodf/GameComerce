namespace GameCommerce.Aplicacao.Dtos
{
    public class TransacaoPagamentoDto
    {
        public int Id { get; set; }
        public int PedidoId { get; set; }
        public int Amount { get; set; }
        public string PaymentMethod { get; set; } = "Pix";
        public string? CustomerName { get; set; }
        public string? CustomerEmail { get; set; }
        public string? CustomerPhone { get; set; }
        public string? CustomerDocument { get; set; }
        public string? ZipCode { get; set; }
        public string? Street { get; set; }
        public string? Number { get; set; }
        public string? Neighborhood { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string Country { get; set; } = "BR";
        public string TransactionId { get; set; }
        public string? Status { get; set; } // "pending", "paid", "expired"
        public string? PixCode { get; set; }
        public string? PostbackUrl { get; set; }
        public string? Message { get; set; }
        public bool Success { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataAtualizacao { get; set; }
    }
}