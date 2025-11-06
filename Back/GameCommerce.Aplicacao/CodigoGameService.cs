using GameCommerce.Aplicacao.Interfaces;

namespace GameCommerce.Aplicacao
{
    public class CodigoGameService: ICodigoGameService
    {
        private readonly Random _random = new Random();
        private const string CARACTERES = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private const int TAMANHO_BLOCO = 4;
        private const int QUANTIDADE_BLOCOS = 4;

        public List<string> GerarCodigosGame(int quantidade)
        {
            var codigos = new List<string>();

            for (int i = 0; i < quantidade; i++)
            {
                codigos.Add(GerarCodigoUnico());
            }

            return codigos;
        }

        private string GerarCodigoUnico()
        {
            var blocos = new List<string>();

            for (int i = 0; i < QUANTIDADE_BLOCOS; i++)
            {
                var bloco = new char[TAMANHO_BLOCO];
                for (int j = 0; j < TAMANHO_BLOCO; j++)
                {
                    bloco[j] = CARACTERES[_random.Next(CARACTERES.Length)];
                }
                blocos.Add(new string(bloco));
            }

            return string.Join("-", blocos);
        }
    }
}
