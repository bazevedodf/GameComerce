namespace GameCommerce.Aplicacao.Dtos
{
    public class MarketingTagDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } // Nome amigável da campanha
        public string Identificador { get; set; } // utm_source correspondente
        public string Tipo { get; set; } // "google-tag-manager", "facebook-pixel", "tiktok-pixel"
        public string TagId { get; set; } // "GTM-XXXXXX", "123456789", etc
        public int SiteInfoId { get; set; }
        public bool Ativo { get; set; }
    }
}
