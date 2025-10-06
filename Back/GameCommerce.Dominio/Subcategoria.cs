namespace GameCommerce.Dominio
{
    public class Subcategoria
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public bool Ativo { get; set; } = true;
        public int CategoriaId { get; set; }
        public Categoria Categoria { get; set; }
    }
}
