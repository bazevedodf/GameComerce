using GameCommerce.Aplicacao.Dtos;

namespace GameCommerce.Aplicacao.Interface
{
    public interface IEmailService
    {
        Task<bool> EnviarEmailPagamentoPixAsync(PedidoDto pedido, TransacaoPagamentoDto transacao, SiteInfoDto siteInfo);
        Task<bool> EnviarEmailCodigosJogosAsync(PedidoDto pedido, List<string> codigos, SiteInfoDto siteInfo);

        Task<bool> SendMail(string fromName,
                            string toName,
                            string toEmail,
                            string subject,
                            string body);
    }
}
