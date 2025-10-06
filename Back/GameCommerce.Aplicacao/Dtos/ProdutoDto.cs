namespace GameCommerce.Aplicacao.Dtos
{
    public class ProdutoDto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public decimal Preco { get; set; }
        public decimal PrecoOriginal { get; set; }
        public int Desconto { get; set; }
        public string Imagem { get; set; }
        public decimal Avaliacao { get; set; }
        public int TotalAvaliacoes { get; set; }
        public List<string> Tags { get; set; }
        public bool Ativo { get; set; }
        public bool EmDestaque { get; set; }
        public string Plataforma { get; set; }
        public string Entrega { get; set; }

        // Relacionamentos
        public int CategoriaId { get; set; }
        public CategoriaDto Categoria { get; set; }
    }
}