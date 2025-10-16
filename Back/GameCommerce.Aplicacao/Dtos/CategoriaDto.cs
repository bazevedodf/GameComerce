namespace GameCommerce.Aplicacao.Dtos
{
    public class CategoriaDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public string? Descricao { get; set; }
        public string? Imagem { get; set; }
        public string? Icon { get; set; }
        public bool Ativo { get; set; }
        public int SiteInfoId { get; set; }

        public IEnumerable<SubcategoriaDto>? Subcategorias { get; set; }
    }
}