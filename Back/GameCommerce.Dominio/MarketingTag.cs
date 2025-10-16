namespace GameCommerce.Dominio
{
    public class MarketingTag
    {
        public int Id { get; set; }
        public string Tipo { get; set; } // "google-tag-manager", "facebook-pixel", "tiktok-pixel"
        public string Nome { get; set; } // Nome amigável da campanha
        public string? Identificador { get; set; } // utm_source correspondente
        public string? TagId { get; set; } // "GTM-XXXXXX", "123456789", etc
        public bool Ativo { get; set; }
        public int SiteInfoId { get; set; }
        public SiteInfo SiteInfo { get; set; }
    }
}
