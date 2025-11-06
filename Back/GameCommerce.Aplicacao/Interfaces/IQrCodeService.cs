namespace GameCommerce.Aplicacao.Interfaces
{
    public interface IQrCodeService
    {
        string GerarQrCodeBase64(string data, int pixelsPerModule = 20);
        byte[] GerarQrCodeBytes(string data, int pixelsPerModule = 20);
    }
}
