export interface MarketingTag {
    id: string;
    tipo: 'google-tag-manager' | 'facebook-pixel' | 'tiktok-pixel' | 'google-analytics';
    tagId: string; // GTM-XXXXXX, FB_PIXEL_ID, etc
    identificador: string; // utm_source que vai na URL
    //nome: string; // Nome amigável da campanha
    //ativo: boolean;
}
