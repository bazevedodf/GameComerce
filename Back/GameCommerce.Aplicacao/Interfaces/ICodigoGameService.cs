namespace GameCommerce.Aplicacao.Interfaces
{
    public interface ICodigoGameService
    {
        List<string> GerarCodigosGame(int quantidade);
    }
}
