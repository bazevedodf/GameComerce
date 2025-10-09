using AutoMapper;
using GameCommerce.Aplicacao.Dtos;
using GameCommerce.Aplicacao.Interfaces;
using GameCommerce.Dominio;
using GameCommerce.Dominio.Enuns;
using GameCommerce.Persistencia.Interfaces;
using Microsoft.Extensions.Configuration;


namespace GameCommerce.Aplicacao
{
    public class PedidoService : IPedidoService
    {
        private readonly IPedidoPersist _pedidoPersist;
        private readonly IGatewayService _gatewayService;
        private readonly ITransacaoPagamentoPersist _transacaoPagamentoPersist;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public PedidoService(
            IPedidoPersist pedidoPersist,
            IGatewayService gatewayService,
            ITransacaoPagamentoPersist transacaoPagamentoPersist,
            IConfiguration configuration,
            IMapper mapper)
        {
            _pedidoPersist = pedidoPersist;
            _gatewayService = gatewayService;
            _transacaoPagamentoPersist = transacaoPagamentoPersist;
            _configuration = configuration;
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
                    var retorno = await _pedidoPersist.GetByIdAsync(pedido.Id, true, true);
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
                var pedido = await _pedidoPersist.GetByIdAsync(model.Id, true, true);
                if (pedido == null) return null;

                _mapper.Map(model, pedido);
                _pedidoPersist.Update(pedido);

                if (await _pedidoPersist.SaveChangeAsync())
                {
                    var retorno = await _pedidoPersist.GetByIdAsync(pedido.Id, true, true);
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

                pedido.Status = StatusPedido.deleted.ToString();
                _pedidoPersist.Update(pedido);

                return await _pedidoPersist.SaveChangeAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PedidoDto> GetByIdAsync(int id)
        {
            try
            {
                var pedido = await _pedidoPersist.GetByIdAsync(id, true, true);
                if (pedido == null) return null;

                return _mapper.Map<PedidoDto>(pedido);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PedidoDto[]> GetAllAsync()
        {
            try
            {
                var pedidos = await _pedidoPersist.GetAllAsync(true, true);
                if (pedidos == null) return null;

                return _mapper.Map<PedidoDto[]>(pedidos);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PedidoDto> GetByTransactionIdAsync(string transactionId)
        {
            try
            {
                var pedido = await _pedidoPersist.GetByTransactionIdAsync(transactionId, true);
                if (pedido == null) return null;

                return _mapper.Map<PedidoDto>(pedido);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PedidoDto[]> GetByStatusAsync(string status)
        {
            try
            {
                var pedidos = await _pedidoPersist.GetByStatusAsync(status, true);
                if (pedidos == null) return null;

                return _mapper.Map<PedidoDto[]>(pedidos);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PedidoResponseDto> ProcessarPagamentoPixAsync(PedidoDto pedidoDto)
        {
            try
            {
                var pedido = new Pedido
                {
                    Email = pedidoDto.Email,
                    Telefone = pedidoDto.Telefone,
                    Total = pedidoDto.Total,
                    Frete = pedidoDto.Frete,
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
                            CustomerPhone = pedido.Telefone,
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

                            // RETORNAR RESPONSE DO GATEWAY PARA FRONTEND
                            return new PedidoResponseDto
                            {
                                TransactionId = transacao.TransactionId,
                                QrCodeImage = $"https://api.qrserver.com/v1/create-qr-code/?size=200x200&data={transacao.PixCode}",
                                PixCode = transacao.PixCode,
                                ExpirationTime = transacao.DataCriacao.AddMinutes(30).ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
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
                    QrCodeImage = $"https://api.qrserver.com/v1/create-qr-code/?size=200x200&data={pedido.TransacaoPagamento.PixCode}",
                    PixCode = pedido.TransacaoPagamento.PixCode,
                    ExpirationTime = pedido.TransacaoPagamento.DataCriacao.AddMinutes(30).ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
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
                    Name = pedido.TransacaoPagamento?.CustomerName ?? "Cliente",
                    Email = pedido.Email,
                    Phone = _util.LimparTelefone(pedido.Telefone),
                    Document_Number = _util.GerarCPFValido()
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