// QrCodeService.cs
using GameCommerce.Aplicacao.Interfaces;
using QRCoder;

namespace GameCommerce.Aplicacao
{
    public class QrCodeService : IQrCodeService
    {
        public string GerarQrCodeBase64(string data, int pixelsPerModule = 20)
        {
            if (string.IsNullOrEmpty(data))
                return string.Empty;

            try
            {
                using var qrGenerator = new QRCodeGenerator();
                using var qrCodeData = qrGenerator.CreateQrCode(data, QRCodeGenerator.ECCLevel.Q);
                using var qrCode = new BitmapByteQRCode(qrCodeData);
                var qrCodeImageBytes = qrCode.GetGraphic(pixelsPerModule);

                return BytesToBase64(qrCodeImageBytes);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao gerar QR Code: {ex.Message}", ex);
            }
        }

        public byte[] GerarQrCodeBytes(string data, int pixelsPerModule = 20)
        {
            if (string.IsNullOrEmpty(data))
                return Array.Empty<byte>();

            try
            {
                using var qrGenerator = new QRCodeGenerator();
                using var qrCodeData = qrGenerator.CreateQrCode(data, QRCodeGenerator.ECCLevel.Q);
                using var qrCode = new BitmapByteQRCode(qrCodeData);
                return qrCode.GetGraphic(pixelsPerModule);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao gerar QR Code: {ex.Message}", ex);
            }
        }

        private string BytesToBase64(byte[] imageBytes)
        {
            return $"data:image/png;base64,{Convert.ToBase64String(imageBytes)}";
        }
    }
}