namespace GameCommerce.Aplicacao.Dtos
{
    public class CupomDto
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public bool Valido { get; set; }
        public decimal? ValorDesconto { get; set; }
        public string TipoDesconto { get; set; } // "percentual", "valor_fixo"
        public string MensagemErro { get; set; }
    }
}