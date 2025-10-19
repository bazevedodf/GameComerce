namespace GameCommerce.Aplicacao.Dtos
{
    public class DashboardTotalizadorDto
    {
        public int TotalSitesAtivos { get; set; }
        public int TotalProdutos { get; set; }
        public int TotalPedidos { get; set; }
        public int TotalPedidosPagos { get; set; }
        public int TotalCupons { get; set; }
        public int TotalMarketingTags { get; set; }
    }
}
