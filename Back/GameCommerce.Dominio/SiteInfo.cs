namespace GameCommerce.Dominio
{
    public class SiteInfo
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string? Dominio { get; set; }
        public string? LogoUrl { get; set; }
        public string? Cnpj { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
        public string? Instagram { get; set; }
        public string? Facebook { get; set; }
        public string? Whatsapp { get; set; }
        public string? YouTube { get; set; }
        public bool Ativo { get; set; } = true;

        public IEnumerable<Cupom>? Cupons { get; set; }
        public IEnumerable<Pedido>? Pedidos { get; set; }
        public IEnumerable<Produto>? Produtos { get; set; }
        public IEnumerable<Categoria>? Categorias { get; set; }
    }
}
