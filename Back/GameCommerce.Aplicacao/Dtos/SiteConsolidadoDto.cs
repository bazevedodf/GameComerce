namespace GameCommerce.Aplicacao.Dtos
{
    public class SiteConsolidadoDto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Dominio { get; set; }
        public bool Status { get; set; }
        public int TotalProdutos { get; set; }
        public int TotalCupons { get; set; }
        public int TotalPedidos { get; set; }
        public int TotalPagos { get; set; }
    }
}
