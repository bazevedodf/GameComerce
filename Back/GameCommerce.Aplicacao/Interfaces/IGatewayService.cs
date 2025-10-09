using GameCommerce.Aplicacao.Dtos;

namespace GameCommerce.Aplicacao.Interfaces
{
    public interface IGatewayService
    {
        Task<NewAgeResponseDto> ProcessarPagamentoPixAsync(NewAgeRequestDto request, SiteInfoDto siteInfo);
        Task<NewAgeResponseDto> ConsultarStatusPagamentoAsync(string transactionId, SiteInfoDto siteInfo);
    }
}
