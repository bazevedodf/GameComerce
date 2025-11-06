using AutoMapper;
using GameCommerce.Aplicacao.Dtos;
using GameCommerce.Aplicacao.Helpers;
using GameCommerce.Aplicacao.Interface;
using GameCommerce.Aplicacao.Interfaces;
using GameCommerce.Dominio;
using GameCommerce.Dominio.Enuns;
using GameCommerce.Persistencia.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Text.RegularExpressions;
using GameCommerce.Aplicacao.Extensions;


namespace GameCommerce.Aplicacao
{
    public class PedidoService : IPedidoService
    {
        private readonly IPedidoPersist _pedidoPersist;
        private readonly IGatewayService _gatewayService;
        private readonly ITransacaoPagamentoPersist _transacaoPagamentoPersist;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        private readonly ICodigoGameService _codigoGameService;
        private readonly IQrCodeService _qrCodeService;
        private readonly IMapper _mapper;

        public PedidoService(
            IPedidoPersist pedidoPersist,
            IGatewayService gatewayService,
            ITransacaoPagamentoPersist transacaoPagamentoPersist,
            IConfiguration configuration,
            IEmailService emailService,
            ICodigoGameService codigoGameService,
            IQrCodeService qrCodeService,
            IMapper mapper)
        {
            _pedidoPersist = pedidoPersist;
            _gatewayService = gatewayService;
            _transacaoPagamentoPersist = transacaoPagamentoPersist;
            _configuration = configuration;
            _emailService = emailService;
            _codigoGameService = codigoGameService;
            _qrCodeService = qrCodeService;
            _mapper = mapper;
        }

        public async Task<PedidoDto> AddAsync(PedidoDto model)
        {
            try
            {
                var pedido = _mapper.Map<Pedido>(model);
                pedido.DataCriacao = DateTime.UtcNow;

                _pedidoPersist.Add(pedido);

                if (await _pedidoPersist.SaveChangeAsync())
                {
                    var retorno = await _pedidoPersist.GetByIdAsync(pedido.Id, true);
                    return _mapper.Map<PedidoDto>(retorno);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PedidoDto> UpdateAsync(PedidoDto model)
        {
            try
            {
                var pedido = await _pedidoPersist.GetByIdAsync(model.Id, true);
                if (pedido == null) return null;

                _mapper.Map(model, pedido);
                _pedidoPersist.Update(pedido);

                if (await _pedidoPersist.SaveChangeAsync())
                {
                    var retorno = await _pedidoPersist.GetByIdAsync(pedido.Id, true);
                    return _mapper.Map<PedidoDto>(retorno);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var pedido = await _pedidoPersist.GetByIdAsync(id);
                if (pedido == null) return false;

                _pedidoPersist.Delete(pedido);

                return await _pedidoPersist.SaveChangeAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        public async Task<PedidoDto> GetByIdAsync(int id, bool includeItens = true)
        {
            try
            {
                var pedido = await _pedidoPersist.GetByIdAsync(id, includeItens);
                if (pedido == null) return null;

                return _mapper.Map<PedidoDto>(pedido);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PedidoDto[]> GetAllAsync(bool includeItens = true)
        {
            try
            {
                var pedidos = await _pedidoPersist.GetAllAsync(includeItens);
                if (pedidos == null) return null;

                return _mapper.Map<PedidoDto[]>(pedidos);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PedidoDto> GetByTransactionIdAsync(string transactionId, bool includeItens = true)
        {
            try
            {
                var pedido = await _pedidoPersist.GetByTransactionIdAsync(transactionId, includeItens);
                if (pedido == null) return null;

                return _mapper.Map<PedidoDto>(pedido);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<PedidoDto[]> GetAllBySiteInfoIdAsync(int siteInfoId, bool includeItens = true)
        {
            try
            {
                var pedidos = await _pedidoPersist.GetAllBySiteInfoIdAsync(siteInfoId, true);
                if (pedidos == null) return null;

                return _mapper.Map<PedidoDto[]>(pedidos);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<PedidoDto[]> GetByStatusAsync(string status, bool includeItens = true)
        {
            try
            {
                var pedidos = await _pedidoPersist.GetByStatusAsync(status, includeItens);
                if (pedidos == null) return null;

                return _mapper.Map<PedidoDto[]>(pedidos);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //Metodos Paginados
        public async Task<PagedResponse<PedidoDto>> GetPaginatedBySiteInfoIdAsync(int page = 1, int pageSize = 10, int? siteInfoId = null, bool includeItens = true)
        {
            try
            {
                // Buscar dados paginados do banco
                var pedidos = await _pedidoPersist.GetPaginatedBySiteInfoIdAsync(page, pageSize, siteInfoId, includeItens);
                var totalItems = await _pedidoPersist.GetCountAsync(siteInfoId);
                var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

                if (pedidos == null || !pedidos.Any())
                    return new PagedResponse<PedidoDto>
                    {
                        Data = new List<PedidoDto>(),
                        Pagination = new PaginationInfo
                        {
                            CurrentPage = page,
                            TotalPages = totalPages,
                            TotalItems = totalItems,
                            PageSize = pageSize
                        }
                    };

                var pedidosDto = _mapper.Map<PedidoDto[]>(pedidos);

                return new PagedResponse<PedidoDto>
                {
                    Data = pedidosDto.ToList(),
                    Pagination = new PaginationInfo
                    {
                        CurrentPage = page,
                        TotalPages = totalPages,
                        TotalItems = totalItems,
                        PageSize = pageSize
                    }
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //Metodos Quantitativos
        public async Task<int> GetCountAsync(int? siteInfoId = null)
        {
            try
            {
                return await _pedidoPersist.GetCountAsync(siteInfoId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao contar pedidos: {ex.Message}");
            }
        }

        public async Task<int> GetCountPagosAsync(int? siteInfoId = null)
        {
            try
            {
                return await _pedidoPersist.GetCountPagosAsync(siteInfoId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao contar pedidos pagos: {ex.Message}");
            }
        }


        // Métodos específicos para PIX - AGORA RETORNAM PedidoResponseDto
        public async Task<PedidoResponseDto> ProcessarPagamentoPixAsync(PedidoDto pedidoDto)
        {
            try
            {
                var pedido = new Pedido
                {
                    Nome = pedidoDto.Nome,
                    Email = pedidoDto.Email,
                    Telefone = pedidoDto.Telefone,
                    CPF = string.IsNullOrEmpty(pedidoDto.CPF) ? null : pedidoDto.CPF,
                    Total = pedidoDto.Total,
                    Frete = pedidoDto.Frete,
                    CupomId = pedidoDto.CupomId,
                    DescontoAplicado = pedidoDto.DescontoAplicado,
                    DataCriacao = DateTime.UtcNow,
                    MeioPagamento = MeioPagamento.Pix,
                    SiteInfoId = (int)pedidoDto.SiteInfoId,
                    Itens = pedidoDto.Itens.Select(itemDto => new ItemPedido
                    {
                        ProdutoId = itemDto.ProdutoId,
                        Quantidade = itemDto.Quantidade,
                        PrecoUnitario = itemDto.PrecoUnitario,
                        Subtotal = itemDto.Quantidade * itemDto.PrecoUnitario
                    }).ToList()
                };

                _pedidoPersist.Add(pedido);

                if (await _pedidoPersist.SaveChangeAsync())
                {
                    var retorno = await _pedidoPersist.GetByIdAsync(pedido.Id, true);

                    var gatewayRequest = await CriarGatewayRequestAsync(retorno);

                    var gatewayResponse = await _gatewayService.ProcessarPagamentoPixAsync(gatewayRequest, pedidoDto.SiteInfo);

                    if (gatewayResponse.Success)
                    {
                        // CRIAR TRANSAÇÃO COM DADOS DO GATEWAY
                        var transacao = new TransacaoPagamento
                        {
                            PedidoId = pedido.Id,
                            TransactionId = gatewayResponse.Data.Transaction_Id,
                            Amount = (int)(pedido.Total * 100),
                            PaymentMethod = "pix",
                            CustomerName = gatewayRequest.Customer?.Name,
                            CustomerEmail = pedido.Email,
                            CustomerPhone = gatewayRequest.Customer?.Phone,
                            CustomerDocument = gatewayRequest.Customer?.Document_Number,
                            Status = gatewayResponse.Data.Status,
                            PixCode = gatewayResponse.Data.Pix_Code,
                            Success = gatewayResponse.Success,
                            Message = gatewayResponse.Message,
                            DataCriacao = DateTime.UtcNow
                        };

                        _transacaoPagamentoPersist.Add(transacao);
                        if (await _transacaoPagamentoPersist.SaveChangeAsync())
                        {
                            pedido.Status = transacao.Status;
                            pedido.TransacaoId = transacao.Id;
                            await _pedidoPersist.SaveChangeAsync();

                            try
                            {
                                var transacaoDto = _mapper.Map<TransacaoPagamentoDto>(transacao);
                                var pedidoCompletoDto = _mapper.Map<PedidoDto>(retorno);
                                await _emailService.EnviarEmailPagamentoPixAsync(pedidoCompletoDto, transacaoDto, pedidoDto.SiteInfo);
                            }
                            catch (Exception)
                            {

                            }

                            // RETORNAR RESPONSE DO GATEWAY PARA FRONTEND
                            return new PedidoResponseDto
                            {
                                TransactionId = transacao.TransactionId,
                                QrCodeImage = _qrCodeService.GerarQrCodeBase64(transacao.PixCode), // $"https://api.qrserver.com/v1/create-qr-code/?size=200x200&data={transacao.PixCode}",
                                PixCode = transacao.PixCode,
                                ExpirationTime = transacao.DataCriacao.AddSeconds(120).ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                                Status = transacao.Status
                            };
                        }
                        else
                        {
                            _pedidoPersist.Delete(pedido);
                            await _pedidoPersist.SaveChangeAsync();
                        }

                        return null;


                    }
                    else
                    {
                        throw new Exception($"Gateway retornou erro: {gatewayResponse.Message}");
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> ProcessarPagamentoConfirmadoAsync(string transactionId)
        {
            try
            {
                var pedido = await _pedidoPersist.GetByTransactionIdAsync(transactionId, true);
                if (pedido == null) return false;

                // Atualizar status do pedido
                pedido.Status = "paid";
                pedido.TransacaoPagamento.Status = "paid";
                pedido.TransacaoPagamento.DataAtualizacao = DateTime.UtcNow;

                _pedidoPersist.Update(pedido);
                await _pedidoPersist.SaveChangeAsync();

                // Gerar códigos para os itens do pedido
                var totalItens = pedido.Itens.Sum(item => item.Quantidade);
                var codigos = _codigoGameService.GerarCodigosGame(totalItens);

                // Enviar email com códigos
                var pedidoDto = _mapper.Map<PedidoDto>(pedido);
                var siteInfo = await _pedidoPersist.GetByIdAsync(pedido.Id);


                try
                {
                    await _emailService.EnviarEmailCodigosJogosAsync(pedidoDto, codigos, _mapper.Map<SiteInfoDto>(siteInfo));
                }
                catch (Exception ex)
                {
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao processar pagamento confirmado: {ex.Message}", ex);
            }
        }

        public async Task<bool> ProcessarWebhookPixAsync(string webhookData)
        {
            try
            {
                //var pedido = await _pedidoPersist.GetByTransactionIdAsync(transactionId, true);
                //if (pedido == null) return false;
                //pedido.Status = status;
                //_pedidoPersist.Update(pedido);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PedidoResponseDto> VerificarStatusPagamentoAsync(string transactionId, bool includeItens = true)
        {
            try
            {
                var pedido = await _pedidoPersist.GetByTransactionIdAsync(transactionId, true);
                if (pedido == null) return null;

                // TODO: Implementar verificação real com gateway
                // Por enquanto, retornar o status atual
                return new PedidoResponseDto
                {
                    TransactionId = pedido.TransacaoPagamento.TransactionId,
                    QrCodeImage = _qrCodeService.GerarQrCodeBase64(pedido.TransacaoPagamento.PixCode),//$"https://api.qrserver.com/v1/create-qr-code/?size=200x200&data={pedido.TransacaoPagamento.PixCode}",
                    PixCode = pedido.TransacaoPagamento.PixCode,
                    ExpirationTime = pedido.TransacaoPagamento.DataCriacao.AddSeconds(120).ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                    Status = pedido.TransacaoPagamento.Status
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task<NewAgeRequestDto> CriarGatewayRequestAsync(Pedido pedido)
        {
            Util _util = new Util(_configuration);
            
            var baseUrl = _util.ObterBaseUrl();

            return new NewAgeRequestDto
            {
                
                Amount = (int)(pedido.Total * 100), // Total em centavos
                PaymentMethod = "pix",
                PostbackUrl = baseUrl != "" ? $"{_util.ObterBaseUrl()}/api/v1/pedidos/webhook/pix": "",
                Customer = new Customer
                {
                    Name = pedido.Nome ?? "Cliente",
                    Email = pedido.Email,
                    Phone = pedido.Telefone.ApenasNumeros(),
                    Document_Number = pedido.CPF.ApenasNumeros() ?? _util.GerarCPFValido()
                },
                Address = new Address
                {
                    Street = pedido.TransacaoPagamento?.Street ?? "Rua Padrão",
                    Number = pedido.TransacaoPagamento?.Number ?? "S/N",
                    Neighborhood = pedido.TransacaoPagamento?.Neighborhood ?? "Centro",
                    City = pedido.TransacaoPagamento?.City ?? "São Paulo",
                    State = pedido.TransacaoPagamento?.State ?? "SP",
                    ZipCode = pedido.TransacaoPagamento?.ZipCode ?? "00000000",
                    Country = "BR"
                },
                Items = pedido.Itens.Select(item =>
                {
                    return new Item
                    {
                        Name = item.Produto.Nome ?? "Produto",
                        Amount = (int)(item.PrecoUnitario * 100), // Preço unitário em centavos
                        Quantity = item.Quantidade,
                        Description = item.Produto.Descricao ?? string.Empty
                    };
                }).ToList()
            };
        }

        

    }
}