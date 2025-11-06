using GameCommerce.Aplicacao.Dtos;
using GameCommerce.Aplicacao.Interface;
using GameCommerce.Aplicacao.Interfaces;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace GameCommerce.Aplicacao
{
    public class EmailService : IEmailService
    {
        private readonly SmtpConfig _smtpConfig;
        private readonly IQrCodeService _qrCodeService;

        public EmailService(SmtpConfig smtpConfig, 
                            IQrCodeService qrCodeService)
        {
            _smtpConfig = smtpConfig;
            _qrCodeService = qrCodeService;
        }

        public async Task<bool> SendMail(string fromName, string toName, string toEmail, string subject, string body)
        {
            try
            {
                if (!_smtpConfig.Ativo)
                    return true;

                using var smtpClient = new SmtpClient(_smtpConfig.Host, _smtpConfig.Port);
                smtpClient.Credentials = new NetworkCredential(_smtpConfig.UserName, _smtpConfig.Password);
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.EnableSsl = true;

                using var mail = new MailMessage();
                mail.From = new MailAddress(_smtpConfig.Email, fromName);
                mail.To.Add(new MailAddress(toEmail, toName));
                mail.Subject = subject;
                mail.SubjectEncoding = Encoding.GetEncoding("ISO-8859-1");
                mail.Body = body;
                mail.BodyEncoding = Encoding.GetEncoding("ISO-8859-1");
                mail.IsBodyHtml = true;

                await smtpClient.SendMailAsync(mail);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao enviar email: {ex.Message}", ex);
            }
        }

        public async Task<bool> EnviarEmailPagamentoPixAsync(PedidoDto pedido, TransacaoPagamentoDto transacao, SiteInfoDto siteInfo)
        {
            var subject = $"{siteInfo.Nome} - Pagamento PIX Pendente - Pedido #{pedido.Id}";
            var body = CriarTemplateEmailPix(pedido, transacao, siteInfo);

            return await SendMail(
                siteInfo.Nome,
                pedido.Nome,
                pedido.Email,
                subject,
                body
            );
        }

        public async Task<bool> EnviarEmailCodigosJogosAsync(PedidoDto pedido, List<string> codigos, SiteInfoDto siteInfo)
        {
            var subject = $"{siteInfo.Nome} - Seus Códigos de Jogo - Pedido #{pedido.Id}";
            var body = CriarTemplateEmailCodigos(pedido, codigos, siteInfo);

            return await SendMail(
                siteInfo.Nome,
                pedido.Nome,
                pedido.Email,
                subject,
                body
            );
        }

        private string CriarTemplateEmailPix(PedidoDto pedido, TransacaoPagamentoDto transacao, SiteInfoDto siteInfo)
        {
            return $@"
                        <!DOCTYPE html>
                        <html lang='pt-BR'>
                        <head>
                            <meta charset='UTF-8'>
                            <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                            <title>Pagamento PIX</title>
                            <style>
                                * {{ margin: 0; padding: 0; box-sizing: border-box; }}
                                body {{ font-family: 'Segoe UI', Arial, sans-serif; line-height: 1.6; color: #333; background: #f8f9fa; }}
                                .container {{ max-width: 600px; margin: 0 auto; background: #ffffff; }}
                                .header {{ background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); padding: 30px; text-align: center; color: white; }}
                                .logo {{ font-size: 28px; font-weight: bold; margin-bottom: 10px; }}
                                .content {{ padding: 30px; }}
                                .section {{ margin-bottom: 25px; padding: 20px; background: #f8f9fa; border-radius: 8px; border-left: 4px solid #667eea; }}
                                .pix-info {{ text-align: center; background: #e8f5e8; border-left-color: #28a745; }}
                                .qr-code {{ max-width: 200px; margin: 15px auto; display: block; }}
                                .copy-code {{ background: #fff; padding: 15px; border-radius: 6px; border: 1px solid #ddd; margin: 15px 0; font-family: monospace; word-break: break-all; }}
                                .btn-copy {{ background: #667eea; color: white; border: none; padding: 8px 16px; border-radius: 4px; cursor: pointer; margin-top: 10px; }}
                                .order-details {{ background: #e3f2fd; border-left-color: #2196f3; }}
                                .product-list {{ margin-top: 15px; }}
                                .product-item {{ display: flex; justify-content: space-between; padding: 8px 0; border-bottom: 1px solid #eee; }}
                                .footer {{ background: #343a40; color: white; padding: 20px; text-align: center; font-size: 14px; }}
                                .steps {{ display: flex; justify-content: space-between; margin: 20px 0; }}
                                .step {{ text-align: center; flex: 1; padding: 10px; }}
                                .step-number {{ background: #667eea; color: white; width: 30px; height: 30px; border-radius: 50%; display: inline-flex; align-items: center; justify-content: center; margin-bottom: 8px; }}
                                @media (max-width: 600px) {{
                                    .steps {{ flex-direction: column; }}
                                    .step {{ margin-bottom: 15px; }}
                                }}
                            </style>
                        </head>
                        <body>
                            <div class='container'>
                                <div class='header'>
                                    <div class='logo'>{siteInfo.Nome}</div>
                                    <h1>Pagamento via PIX</h1>
                                </div>
        
                                <div class='content'>
                                    <div class='section'>
                                        <h2>Olá, {pedido.Nome}!</h2>
                                        <p>Seu pedido <strong>#{pedido.TransacaoPagamento}</strong> foi criado com sucesso. Para finalizar sua compra, realize o pagamento PIX abaixo.</p>
                                    </div>

                                    <div class='section pix-info'>
                                        <h3>💰 Pagamento PIX</h3>
                                        <p><strong>Valor: R$ {pedido.Total + pedido.Frete - (pedido.DescontoAplicado ?? 0):N2}</strong></p>
                
                                        {(transacao.PixCode != null ? $@"
                                        <div style='text-align: center;'>
                                            <p><strong>QR Code para pagamento:</strong></p>
                                            <div class='qr-code'>
                                                <img src='{_qrCodeService.GerarQrCodeBase64(transacao.PixCode)}' alt='QR Code PIX' style='max-width: 200px; height: auto;' />
                                            </div>
        
                                            <p><strong>Ou copie o código PIX:</strong></p>
                                            <div class='copy-code'>
                                                {transacao.PixCode}
                                            </div>
                                            <button class='btn-copy' onclick='navigator.clipboard.writeText(""{transacao.PixCode}"")'>Copiar Código</button>
                                        </div>
                                        " : "<p style='color: #dc3545;'>Código PIX não disponível. Entre em contato com o suporte.</p>")}

                                        <div class='steps'>
                                            <div class='step'>
                                                <div class='step-number'>1</div>
                                                <p>Abra seu app do banco</p>
                                            </div>
                                            <div class='step'>
                                                <div class='step-number'>2</div>
                                                <p>Escaneie o QR Code ou cole o código</p>
                                            </div>
                                            <div class='step'>
                                                <div class='step-number'>3</div>
                                                <p>Confirme o pagamento</p>
                                            </div>
                                        </div>
                                    </div>

                                    <div class='section order-details'>
                                        <h3>📦 Detalhes do Pedido</h3>
                                        <p><strong>Número do Pedido:</strong> #{pedido.Id}</p>
                                        <p><strong>Data:</strong> {pedido.DataCriacao:dd/MM/yyyy HH:mm}</p>
                                        <p><strong>Status:</strong> <span style='color: #ffc107; font-weight: bold;'>Aguardando Pagamento</span></p>
                
                                        <div class='product-list'>
                                            <h4>Itens do Pedido:</h4>
                                            {string.Join("", pedido.Itens?.Select(item => $@"
                                            <div class='product-item'>
                                                <span>{item.Produto?.Nome} x {item.Quantidade}</span>
                                                <span>R$ {item.Subtotal:N2}</span>
                                            </div>
                                            ") ?? new List<string>())}
                    
                                            <div class='product-item' style='border-top: 2px solid #ccc; padding-top: 10px; font-weight: bold;'>
                                                <span>Total</span>
                                                <span>R$ {pedido.Total + pedido.Frete - (pedido.DescontoAplicado ?? 0):N2}</span>
                                            </div>
                                        </div>
                                    </div>

                                    <div class='section'>
                                        <h3>📞 Precisa de Ajuda?</h3>
                                        <p>Entre em contato conosco:</p>
                                        <p><strong>WhatsApp:</strong> {siteInfo.Whatsapp}</p>
                                        <p><strong>Email:</strong> {siteInfo.Email}</p>
                                    </div>
                                </div>

                                <div class='footer'>
                                    <p>&copy; {DateTime.Now.Year} {siteInfo.Nome} - Todos os direitos reservados</p>
                                    <p>{siteInfo.Cnpj} | {siteInfo.Address}</p>
                                </div>
                            </div>
                        </body>
                        </html>";
        }

        private string CriarTemplateEmailCodigos(PedidoDto pedido, List<string> codigos, SiteInfoDto siteInfo)
        {
            return $@"
                    <!DOCTYPE html>
                    <html lang='pt-BR'>
                    <head>
                        <meta charset='UTF-8'>
                        <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                        <title>Seus Códigos de Jogo</title>
                        <style>
                            * {{ margin: 0; padding: 0; box-sizing: border-box; }}
                            body {{ font-family: 'Segoe UI', Arial, sans-serif; line-height: 1.6; color: #333; background: #f8f9fa; }}
                            .container {{ max-width: 600px; margin: 0 auto; background: #ffffff; }}
                            .header {{ background: linear-gradient(135deg, #28a745 0%, #20c997 100%); padding: 30px; text-align: center; color: white; }}
                            .logo {{ font-size: 28px; font-weight: bold; margin-bottom: 10px; }}
                            .content {{ padding: 30px; }}
                            .section {{ margin-bottom: 25px; padding: 20px; background: #f8f9fa; border-radius: 8px; border-left: 4px solid #28a745; }}
                            .success-badge {{ background: #28a745; color: white; padding: 10px 20px; border-radius: 20px; display: inline-block; margin-bottom: 15px; }}
                            .code-item {{ background: #fff; padding: 15px; border-radius: 8px; border: 2px dashed #28a745; margin: 10px 0; text-align: center; }}
                            .code {{ font-family: 'Courier New', monospace; font-size: 18px; font-weight: bold; letter-spacing: 2px; color: #28a745; }}
                            .btn-copy {{ background: #28a745; color: white; border: none; padding: 8px 16px; border-radius: 4px; cursor: pointer; margin-top: 10px; }}
                            .instructions {{ background: #e8f5e8; border-left-color: #20c997; }}
                            .footer {{ background: #343a40; color: white; padding: 20px; text-align: center; font-size: 14px; }}
                            .product-info {{ display: flex; justify-content: space-between; align-items: center; margin: 10px 0; }}
                            @media (max-width: 600px) {{
                                .product-info {{ flex-direction: column; align-items: flex-start; }}
                            }}
                        </style>
                    </head>
                    <body>
                        <div class='container'>
                            <div class='header'>
                                <div class='logo'>{siteInfo.Nome}</div>
                                <h1>Pagamento Confirmado! 🎉</h1>
                            </div>
        
                            <div class='content'>
                                <div class='section'>
                                    <div class='success-badge'>✅ Pagamento Aprovado</div>
                                    <h2>Parabéns, {pedido.Nome}!</h2>
                                    <p>Seu pagamento foi confirmado e seus códigos de jogo estão prontos para uso.</p>
                                    <p><strong>Pedido: #{pedido.Id}</strong> | Data: {DateTime.Now:dd/MM/yyyy HH:mm}</p>
                                </div>

                                <div class='section'>
                                    <h3>🎮 Seus Códigos de Jogo</h3>
                                    <p>Abaixo estão os códigos que você adquiriu:</p>
                
                                    {string.Join("", pedido.Itens?.Select((item, index) => $@"
                                    <div class='code-item'>
                                        <div class='product-info'>
                                            <div>
                                                <strong>{item.Produto?.Nome}</strong>
                                                <br>
                                                <small>Quantidade: {item.Quantidade}</small>
                                            </div>
                                            <div style='text-align: right;'>
                                                <strong>R$ {item.Subtotal:N2}</strong>
                                            </div>
                                        </div>
                                        {(index < codigos.Count ? $@"
                                        <div class='code'>{codigos[index]}</div>
                                        <button class='btn-copy' onclick='navigator.clipboard.writeText(""{codigos[index]}"")'>Copiar Código</button>
                                        " : "<p style='color: #dc3545;'>Código não disponível</p>")}
                                    </div>
                                    ") ?? new List<string>())}
                                </div>

                                <div class='section instructions'>
                                    <h3>📋 Como Usar Seus Códigos</h3>
                                    <ol style='margin-left: 20px; margin-top: 10px;'>
                                        <li>Copie o código desejado</li>
                                        <li>Abra o jogo correspondente</li>
                                        <li>Vá até a seção de resgate de códigos</li>
                                        <li>Cole o código e confirme</li>
                                        <li>Seus créditos serão adicionados automaticamente</li>
                                    </ol>
                
                                    <div style='margin-top: 15px; padding: 15px; background: #d4edda; border-radius: 6px;'>
                                        <strong>💡 Dica Importante:</strong>
                                        <p style='margin: 5px 0 0 0; font-size: 14px;'>
                                            Cada código pode ser utilizado apenas uma vez. Mantenha-os em local seguro.
                                        </p>
                                    </div>
                                </div>

                                <div class='section'>
                                    <h3>❓ Precisa de Ajuda?</h3>
                                    <p>Se encontrar qualquer problema ao resgatar seus códigos:</p>
                                    <p><strong>WhatsApp:</strong> {siteInfo.Whatsapp}</p>
                                    <p><strong>Email:</strong> {siteInfo.Email}</p>
                                    <p style='margin-top: 10px; font-size: 14px; color: #666;'>
                                        Inclua o número do pedido (#{pedido.Id}) em seu contato para agilizarmos o atendimento.
                                    </p>
                                </div>
                            </div>

                            <div class='footer'>
                                <p>&copy; {DateTime.Now.Year} {siteInfo.Nome} - Todos os direitos reservados</p>
                                <p>{siteInfo.Cnpj} | {siteInfo.Address}</p>
                            </div>
                        </div>
                    </body>
                    </html>";
        }
    }
}