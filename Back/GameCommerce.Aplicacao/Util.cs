using Microsoft.Extensions.Configuration;

namespace GameCommerce.Aplicacao
{
    public class Util
    {
        private readonly IConfiguration _configuration;

        public Util(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        public string GerarCPFValido()
        {
            var random = new Random();

            // Gerar 9 números aleatórios
            int[] cpf = new int[9];
            for (int i = 0; i < 9; i++)
            {
                cpf[i] = random.Next(0, 10);
            }

            // Calcular primeiro dígito
            int soma = 0;
            for (int i = 0; i < 9; i++)
            {
                soma += cpf[i] * (10 - i);
            }
            int digito1 = soma % 11 < 2 ? 0 : 11 - (soma % 11);

            // Calcular segundo dígito
            soma = 0;
            for (int i = 0; i < 9; i++)
            {
                soma += cpf[i] * (11 - i);
            }
            soma += digito1 * 2;
            int digito2 = soma % 11 < 2 ? 0 : 11 - (soma % 11);

            // Retornar apenas números (formato que o gateway espera)
            return $"{cpf[0]}{cpf[1]}{cpf[2]}{cpf[3]}{cpf[4]}{cpf[5]}{cpf[6]}{cpf[7]}{cpf[8]}{digito1}{digito2}";
        }

        public string ObterBaseUrl()
        {
            var baseUrl = Environment.GetEnvironmentVariable("ASPNETCORE_URLS")?.Split(';').FirstOrDefault();
            if (!string.IsNullOrEmpty(baseUrl))
                return baseUrl.TrimEnd('/');

            baseUrl = _configuration["BaseUrl"];
            if (!string.IsNullOrEmpty(baseUrl))
                return baseUrl.TrimEnd('/');

            return "";
        }

        public string LimparTelefone(string telefone)
        {
            if (string.IsNullOrWhiteSpace(telefone))
                return string.Empty;

            // Remove todos os caracteres não numéricos
            var apenasNumeros = new string(telefone.Where(c => char.IsDigit(c)).ToArray());

            // Remove zeros à esquerda se houver
            return apenasNumeros.TrimStart('0');
        }
    }
}
