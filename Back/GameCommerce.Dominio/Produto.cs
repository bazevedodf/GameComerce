using GameCommerce.Dominio.Enuns;

namespace GameCommerce.Dominio
{
    public class Produto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public decimal Preco { get; set; }
        public decimal PrecoOriginal { get; set; }
        public int Desconto { get; set; }
        public string Imagem { get; set; }
        public int SiteInfoId { get; set; }
        public SiteInfo SiteInfo { get; set; }
        public int CategoriaId { get; set; }
        public Categoria Categoria { get; set; }
        public decimal Avaliacao { get; set; }
        public int TotalAvaliacoes { get; set; }
        public List<string> Tags { get; set; }
        public bool Ativo { get; set; } = true;
        public bool EmDestaque { get; set; }
        public TipoEntrega Entrega { get; set; }

    }
}
