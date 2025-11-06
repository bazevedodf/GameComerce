namespace GameCommerce.Aplicacao.Dtos
{
    public class SmtpConfig
    {
        public bool Ativo { get; set; } = false;
        public string Host { get; set; }
        public int Port { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
