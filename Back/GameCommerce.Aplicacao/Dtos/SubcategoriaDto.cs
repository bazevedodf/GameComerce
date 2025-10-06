namespace GameCommerce.Aplicacao.Dtos
{
    public class SubcategoriaDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public bool Ativo { get; set; }
        public CategoriaDto Categoria { get; set; }
    }
}