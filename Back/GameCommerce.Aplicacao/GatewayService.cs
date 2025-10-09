using GameCommerce.Aplicacao.Dtos;
using GameCommerce.Aplicacao.Interfaces;
using System.Text;
using System.Text.Json;

namespace GameCommerce.Aplicacao
{
    public class GatewayService : IGatewayService
    {
        private readonly HttpClient _httpClient;

        public GatewayService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<NewAgeResponseDto> ProcessarPagamentoPixAsync(NewAgeRequestDto request, SiteInfoDto siteInfo)
        {
            try
            {
                // Configurar headers de autenticação
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("secret-key", $"{siteInfo.ApiKey}");
                //_httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

                // Serializar request
                var jsonRequest = JsonSerializer.Serialize(request, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

                // Fazer requisição para o gateway
                var response = await _httpClient.PostAsync(siteInfo.BaseUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var gatewayResponse = JsonSerializer.Deserialize<NewAgeResponseDto>(jsonResponse, new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = null,
                        PropertyNameCaseInsensitive = true
                    });

                    return gatewayResponse;
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Erro no gateway: {response.StatusCode} - {errorContent}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao processar pagamento no gateway: {ex.Message}");
            }
        }

        public async Task<NewAgeResponseDto> ConsultarStatusPagamentoAsync(string transactionId, SiteInfoDto siteInfo)
        {
            try
            {
                // Configurar headers de autenticação
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("secret-key", $"{siteInfo.ApiKey}");
                _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

                // Fazer requisição para consultar status
                var response = await _httpClient.GetAsync($"{siteInfo.BaseUrl}/{transactionId}");

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var gatewayResponse = JsonSerializer.Deserialize<NewAgeResponseDto>(jsonResponse, new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    });

                    return gatewayResponse;
                }
                else
                {
                    throw new Exception($"Erro ao consultar status: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao consultar status no gateway: {ex.Message}");
            }
        }
    }
}
