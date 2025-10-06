using GameCommerce.Dominio.Enuns;

namespace GameCommerce.Dominio
{
    public class Cupom
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public bool Valido { get; set; }
        public decimal? ValorDesconto { get; set; }
        public TipoDesconto TipoDesconto { get; set; }
        public string? MensagemErro { get; set; }
        public bool Ativo { get; set; } = true;
        public int SiteInfoId { get; set; }
        public SiteInfo SiteInfo { get; set; }

        public IEnumerable<Pedido>? Pedidos { get; set; }
    }
}
