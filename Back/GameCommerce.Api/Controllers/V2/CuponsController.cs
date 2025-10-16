using GameCommerce.Aplicacao.Dtos;
using GameCommerce.Aplicacao.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GameCommerce.Api.Controllers.V2
{
    [ApiController]
    [Route("api/v2/[controller]")]
    [ApiExplorerSettings(GroupName = "v2")]
    public class CuponsController : ControllerBase
    {
        private readonly ICupomService _cupomService;

        public CuponsController(ICupomService cupomService)
        {
            _cupomService = cupomService;
        }

        /// <summary>
        /// Obter todos os cupons (Admin)
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll(bool apenasAtivos = true)
        {
            try
            {
                var cupons = await _cupomService.GetAllAsync(apenasAtivos);
                if (cupons == null || !cupons.Any())
                    return NotFound("Nenhum cupom encontrado");

                return Ok(cupons);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao buscar cupons: {ex.Message}");
            }
        }

        /// <summary>
        /// Obter cupom por ID (Admin)
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var cupom = await _cupomService.GetByIdAsync(id);
                if (cupom == null)
                    return NotFound($"Cupom com ID {id} não encontrado");

                return Ok(cupom);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao buscar cupom: {ex.Message}");
            }
        }

        /// <summary>
        /// Obter cupom por código (Admin)
        /// </summary>
        [HttpGet("codigo/{codigo}")]
        public async Task<IActionResult> GetByCodigo(string codigo)
        {
            try
            {
                var cupom = await _cupomService.GetByCodigoAsync(codigo);
                if (cupom == null)
                    return NotFound($"Cupom com código {codigo} não encontrado");

                return Ok(cupom);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao buscar cupom: {ex.Message}");
            }
        }

        /// <summary>
        /// Validar cupom (Admin)
        /// </summary>
        [HttpGet("validar/{codigo}")]
        public async Task<IActionResult> ValidarCupom(string codigo)
        {
            try
            {
                var cupom = await _cupomService.ValidarCupomAsync(codigo);
                if (cupom == null)
                    return NotFound($"Cupom com código {codigo} não encontrado");

                return Ok(cupom);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao validar cupom: {ex.Message}");
            }
        }

        /// <summary>
        /// Criar novo cupom (Admin)
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create(CupomDto cupomDto)
        {
            try
            {
                var cupomCriado = await _cupomService.AddAsync(cupomDto);
                if (cupomCriado == null)
                    return BadRequest("Erro ao criar cupom");

                return CreatedAtAction(nameof(GetById), new { id = cupomCriado.Id }, cupomCriado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao criar cupom: {ex.Message}");
            }
        }

        /// <summary>
        /// Atualizar cupom (Admin)
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CupomDto cupomDto)
        {
            try
            {
                if (id != cupomDto.Id)
                    return BadRequest("ID do cupom não confere");

                var cupomAtualizado = await _cupomService.UpdateAsync(cupomDto);
                if (cupomAtualizado == null)
                    return NotFound($"Cupom com ID {id} não encontrado");

                return Ok(cupomAtualizado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao atualizar cupom: {ex.Message}");
            }
        }

        /// <summary>
        /// Excluir cupom (soft delete - Admin)
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var resultado = await _cupomService.DeleteAsync(id);
                if (!resultado)
                    return NotFound($"Cupom com ID {id} não encontrado");

                return Ok(new { mensagem = "Cupom excluído com sucesso" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao excluir cupom: {ex.Message}");
            }
        }

        /// <summary>
        /// Ativar/Desativar cupom (Admin)
        /// </summary>
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> ToggleStatus(int id, [FromBody] bool ativo)
        {
            try
            {
                var cupom = await _cupomService.GetByIdAsync(id);
                if (cupom == null)
                    return NotFound($"Cupom com ID {id} não encontrado");

                // Para cupons, "ativo" geralmente significa válido para uso
                cupom.Valido = ativo;
                var cupomAtualizado = await _cupomService.UpdateAsync(cupom);

                return Ok(cupomAtualizado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao alterar status do cupom: {ex.Message}");
            }
        }

        /// <summary>
        /// Invalidar cupom (Admin)
        /// </summary>
        [HttpPatch("{id}/invalidar")]
        public async Task<IActionResult> InvalidarCupom(int id)
        {
            try
            {
                var cupom = await _cupomService.GetByIdAsync(id);
                if (cupom == null)
                    return NotFound($"Cupom com ID {id} não encontrado");

                cupom.Valido = false;
                var cupomAtualizado = await _cupomService.UpdateAsync(cupom);

                return Ok(new
                {
                    mensagem = "Cupom invalidado com sucesso",
                    cupom = cupomAtualizado
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao invalidar cupom: {ex.Message}");
            }
        }
    }
}