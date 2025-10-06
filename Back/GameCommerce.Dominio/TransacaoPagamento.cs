namespace GameCommerce.Dominio
{
    public class TransacaoPagamento
    {
        public int Id { get; set; }

        // RELACIONAMENTO COM PEDIDO
        public int PedidoId { get; set; }
        public Pedido Pedido { get; set; }

        // DADOS ENVIADOS AO GATEWAY
        public int Amount { get; set; } // Em centavos (1790 = R$ 17,90)
        public string PaymentMethod { get; set; } = "pix";

        // CUSTOMER (dados enviados)
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerDocument { get; set; }

        // ADDRESS (dados enviados)
        public string ZipCode { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }
        public string Neighborhood { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; } = "BR";

        // RESPOSTA DO GATEWAY
        public string TransactionId { get; set; }
        public string GatewayStatus { get; set; } // "pending", "paid", "expired"
        public string PixCode { get; set; }
        public string GatewayCustomerId { get; set; }
        public string PostbackUrl { get; set; }
        public string GatewayMessage { get; set; }
        public bool GatewaySuccess { get; set; }

        // CONTROLE
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
        public DateTime? DataAtualizacao { get; set; }
    }
}