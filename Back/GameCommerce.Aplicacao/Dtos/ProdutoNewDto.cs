namespace GameCommerce.Aplicacao.Dtos
{
    public class ProdutoNewDto
    {
        public int? Id { get; set; }
        public string Nome { get; set; }
        public string? Descricao { get; set; }
        public decimal Preco { get; set; }
        public decimal PrecoOriginal { get; set; }
        public int Desconto { get; set; }
        public string? Imagem { get; set; }
        public decimal? Avaliacao { get; set; }
        public int TotalAvaliacoes { get; set; }
        public List<string>? Tags { get; set; }
        public bool Ativo { get; set; }
        public bool EmDestaque { get; set; } = false;
        public string Entrega { get; set; }
        public DateTime DataCadastro { get; set; } = DateTime.Now;
        public DateTime? DataAtualizacao { get; set; }

        // Relacionamentos
        public int CategoriaId { get; set; }
        public CategoriaDto? Categoria { get; set; }
        public int SiteInfoId { get; set; }
        public SiteInfoDto? SiteInfo { get; set; }
    }
}