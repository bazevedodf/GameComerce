using AutoMapper;
using GameCommerce.Aplicacao.Dtos;
using GameCommerce.Aplicacao.Interfaces;
using GameCommerce.Dominio;
using GameCommerce.Persistencia.Interfaces;

namespace GameCommerce.Aplicacao
{
    public class CupomService : ICupomService
    {
        private readonly ICupomPersist _cupomPersist;
        private readonly IMapper _mapper;

        public CupomService(ICupomPersist cupomPersist, IMapper mapper)
        {
            _cupomPersist = cupomPersist;
            _mapper = mapper;
        }

        public async Task<CupomDto> AddAsync(CupomDto model)
        {
            try
            {
                var cupom = _mapper.Map<Cupom>(model);
                _cupomPersist.Add(cupom);

                if (await _cupomPersist.SaveChangeAsync())
                {
                    var retorno = await _cupomPersist.GetByIdAsync(cupom.Id);
                    return _mapper.Map<CupomDto>(retorno);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<CupomDto> UpdateAsync(CupomDto model)
        {
            try
            {
                var cupom = await _cupomPersist.GetByIdAsync(model.Id);
                if (cupom == null) return null;

                _mapper.Map(model, cupom);
                _cupomPersist.Update(cupom);

                if (await _cupomPersist.SaveChangeAsync())
                {
                    var retorno = await _cupomPersist.GetByIdAsync(cupom.Id);
                    return _mapper.Map<CupomDto>(retorno);
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
                var cupom = await _cupomPersist.GetByIdAsync(id);
                if (cupom == null) return false;

                cupom.Ativo = false;
                _cupomPersist.Update(cupom);

                return await _cupomPersist.SaveChangeAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<CupomDto> GetByIdAsync(int id)
        {
            try
            {
                var cupom = await _cupomPersist.GetByIdAsync(id);
                if (cupom == null) return null;

                return _mapper.Map<CupomDto>(cupom);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<CupomDto> GetByCodigoAsync(string codigo)
        {
            try
            {
                var cupom = await _cupomPersist.GetByCodigoAsync(codigo);
                if (cupom == null) return null;

                return _mapper.Map<CupomDto>(cupom);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<CupomDto[]> GetAllAsync()
        {
            try
            {
                var cupons = await _cupomPersist.GetAllAsync();
                if (cupons == null) return null;

                return _mapper.Map<CupomDto[]>(cupons);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<CupomDto> ValidarCupomAsync(string codigo)
        {
            try
            {
                var cupom = await _cupomPersist.ValidarCupomAsync(codigo);
                if (cupom == null)
                {
                    // Retorna um cupom inválido com mensagem de erro
                    return new CupomDto
                    {
                        Codigo = codigo.ToUpper(),
                        Valido = false,
                        MensagemErro = "Cupom não encontrado"
                    };
                }

                return _mapper.Map<CupomDto>(cupom);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}