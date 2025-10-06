namespace GameCommerce.Aplicacao.Dtos
{
    public class GatewayPixWebhookRequest
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public GatewayPixWebhookData Data { get; set; }
    }
}
