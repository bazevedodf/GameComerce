using System.ComponentModel.DataAnnotations;

namespace GameCommerce.Aplicacao.Dtos
{
    public class PedidoDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        [StringLength(255, ErrorMessage = "Email deve ter no máximo 255 caracteres")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Telefone é obrigatório")]
        [RegularExpression(@"^\(\d{2}\) \d{5}-\d{4}$", ErrorMessage = "Telefone deve estar no formato (11) 99999-9999")]
        [StringLength(20, ErrorMessage = "Telefone deve ter no máximo 20 caracteres")]
        public string Telefone { get; set; }

        [Required(ErrorMessage = "Total é obrigatório")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Total deve ser maior que zero")]
        public decimal Total { get; set; }

        [Required(ErrorMessage = "Frete é obrigatório")]
        [Range(0, double.MaxValue, ErrorMessage = "Frete não pode ser negativo")]
        public decimal Frete { get; set; }

        public string? Status { get; set; } // "pending", "paid", "expired"

        [Range(0, double.MaxValue, ErrorMessage = "Desconto não pode ser negativo")]
        public decimal? DescontoAplicado { get; set; }

        public DateTime DataCriacao { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Meio de pagamento é obrigatório")]
        [RegularExpression("^(PIX|CartaoCredito|Boleto)$", ErrorMessage = "Meio de pagamento deve ser Pix, CartaoCredito ou Boleto")]
        public string MeioPagamento { get; set; } // "Pix", "CartaoCredito", "Boleto"

        // Relacionamentos
        public int? CupomId { get; set; }
        public CupomDto? Cupom { get; set; }
        public int? SiteInfoId { get; set; }
        public SiteInfoDto? SiteInfo { get; set; }

        [Required(ErrorMessage = "Itens são obrigatórios")]
        [MinLength(1, ErrorMessage = "Pedido deve ter pelo menos 1 item")]
        public List<ItemPedidoDto>? Itens { get; set; }
        public TransacaoPagamentoDto? TransacaoPagamento { get; set; }
    }
}