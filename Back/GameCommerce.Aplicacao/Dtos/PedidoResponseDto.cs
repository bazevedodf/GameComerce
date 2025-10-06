namespace GameCommerce.Aplicacao.Dtos
{
    public class PedidoResponseDto
    {
        public string TransactionId { get; set; }
        public string QrCodeImage { get; set; }
        public string PixCode { get; set; }
        public string ExpirationTime { get; set; }
        public string Status { get; set; } // "pending", "paid", "expired"
    }
}