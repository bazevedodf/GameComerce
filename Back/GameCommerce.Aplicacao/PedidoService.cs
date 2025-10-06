using AutoMapper;
using GameCommerce.Aplicacao.Dtos;
using GameCommerce.Aplicacao.Interfaces;
using GameCommerce.Dominio;
using GameCommerce.Dominio.Enuns;
using GameCommerce.Persistencia.Interfaces;

namespace GameCommerce.Aplicacao
{
    public class PedidoService : IPedidoService
    {
        private readonly IPedidoPersist _pedidoPersist;
        private readonly ITransacaoPagamentoPersist _transacaoPagamentoPersist;
        private readonly IMapper _mapper;

        public PedidoService(
            IPedidoPersist pedidoPersist,
            ITransacaoPagamentoPersist transacaoPagamentoPersist,
            IMapper mapper)
        {
            _pedidoPersist = pedidoPersist;
            _transacaoPagamentoPersist = transacaoPagamentoPersist;
            _mapper = mapper;
        }

        public async Task<PedidoDto> AddAsync(PedidoDto model)
        {
            try
            {
                var pedido = _mapper.Map<Pedido>(model);
                pedido.DataCriacao = DateTime.UtcNow;
                pedido.Status = StatusPedido.Pendente;

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

                pedido.Status = StatusPedido.Cancelado;
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
                if (!Enum.TryParse<StatusPedido>(status, out var statusEnum))
                    return null;

                var pedidos = await _pedidoPersist.GetByStatusAsync(statusEnum, true);
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
                var pedido = _mapper.Map<Pedido>(pedidoDto);
                pedido.DataCriacao = DateTime.UtcNow;
                pedido.Status = StatusPedido.Pendente;
                pedido.MeioPagamento = MeioPagamento.Pix;

                _pedidoPersist.Add(pedido);

                if (await _pedidoPersist.SaveChangeAsync())
                {
                    // Gerar transaction ID único
                    var transactionId = "TX" + DateTime.UtcNow.Ticks;

                    // Criar transação PIX
                    var transacao = new TransacaoPagamento
                    {
                        PedidoId = pedido.Id,
                        TransactionId = transactionId,
                        GatewayStatus = "pending",
                        PixCode = "00020126860014br.gov.bcb.pix2564pix.ecomovi.com.br/qr/v3/at/71ad1e5e-a49d-4b1f-ab00-f82f78e17e8652040000053039865802BR5925KAPTPAY_TECNOLOGIA_DE_PA66009ARAPONGA562070503***630431BD",
                        CustomerName = pedidoDto.TransacaoPagamento?.CustomerName,
                        CustomerEmail = pedido.Email,
                        CustomerPhone = pedido.Telefone,
                        DataCriacao = DateTime.UtcNow
                    };

                    _transacaoPagamentoPersist.Add(transacao);
                    await _transacaoPagamentoPersist.SaveChangeAsync();

                    // Retornar o response específico para o frontend
                    return new PedidoResponseDto
                    {
                        TransactionId = transactionId,
                        QrCodeImage = "assets/img/qr-code-pix.png",
                        PixCode = transacao.PixCode,
                        ExpirationTime = DateTime.UtcNow.AddMinutes(30).ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                        Status = "pending"
                    };
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PedidoResponseDto> VerificarStatusPagamentoAsync(string transactionId)
        {
            try
            {
                var pedido = await _pedidoPersist.GetByTransactionIdAsync(transactionId, true);
                if (pedido == null) return null;

                // TODO: Implementar verificação real com gateway
                // Por enquanto, retornar o status atual
                return new PedidoResponseDto
                {
                    TransactionId = transactionId,
                    QrCodeImage = "assets/img/qr-code-pix.png",
                    PixCode = pedido.TransacaoPagamento?.PixCode,
                    ExpirationTime = pedido.TransacaoPagamento?.DataCriacao.AddMinutes(30).ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                    Status = pedido.TransacaoPagamento?.GatewayStatus ?? "pending"
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}