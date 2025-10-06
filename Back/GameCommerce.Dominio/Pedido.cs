using GameCommerce.Dominio.Enuns;

namespace GameCommerce.Dominio
{
    public class Pedido
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public decimal Total { get; set; }
        public decimal Frete { get; set; }
        public StatusPedido Status { get; set; }
        public decimal? DescontoAplicado { get; set; }
        public DateTime DataCriacao { get; set; }
        public int? CupomId { get; set; }
        public Cupom? Cupom { get; set; }
        public int SiteInfoId { get; set; }
        public SiteInfo SiteInfo { get; set; }
        public MeioPagamento MeioPagamento { get; set; }
        public int? TransacaoId { get; set; }
        public TransacaoPagamento? TransacaoPagamento { get; set; }
        public IEnumerable<ItemPedido> Itens { get; set; }
    }
}
