namespace GameCommerce.Aplicacao.Dtos
{
    public class GatewayPixWebhookData
    {
        public string? TransactionId { get; set; }
        public string? Status { get; set; } // "pending", "paid", "expired", etc.
        public string? PixCode { get; set; }
        public GatewayPixCustomer? Customer { get; set; }
        public string? PostbackUrl { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public decimal? Amount { get; set; }
    }
}
