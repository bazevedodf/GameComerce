using GameCommerce.Dominio;

namespace GameCommerce.Aplicacao.Dtos
{
    public class SiteInfoDto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Dominio { get; set; }
        public string? LogoUrl { get; set; }
        public string Cnpj { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Instagram { get; set; }
        public string Facebook { get; set; }
        public string Whatsapp { get; set; }
        public string? ApiKey { get; set; }
        public string? BaseUrl { get; set; }
        public bool Ativo { get; set; }

        public IEnumerable<CupomDto>? Cupons { get; set; }
        public IEnumerable<PedidoDto>? Pedidos { get; set; }
        public IEnumerable<ProdutoDto>? Produtos { get; set; }
        public IEnumerable<CategoriaDto>? Categorias { get; set; }
        public IEnumerable<MarketingTagDto>? MarketingTags { get; set; }
    }
}