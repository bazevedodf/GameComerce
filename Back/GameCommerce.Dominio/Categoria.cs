namespace GameCommerce.Dominio
{
    public class Categoria
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public string? Descricao { get; set; }
        public string Imagem { get; set; }
        public string? Icon { get; set; }
        public bool Ativo { get; set; } = true;
        public int SiteInfoId { get; set; }
        public SiteInfo SiteInfo { get; set; }
        public IEnumerable<Subcategoria>? Subcategorias { get; set; }
        public IEnumerable<Produto>? Produtos { get; set; }
    }
}
