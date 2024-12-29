using ContaBancaria.DTOs;
using ContaBancaria.Services;
using Microsoft.AspNetCore.Mvc;

namespace ContaBancaria.Controllers
{
    public class SaqueController : ControllerBase
    {
        private readonly ContaService _contaService;


        public SaqueController(ContaService contaService)
        {
            _contaService = contaService;
        }

        [HttpPost("sacar")]
        public async Task<IActionResult> Sacar([FromBody] SaqueDTO saqueDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var resultado = await _contaService.SacarAsync(saqueDto.IdConta, saqueDto.Valor);
                if (resultado)
                    return Ok("Saque realizado com sucesso.");
                return BadRequest("Erro ao realizar saque.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro: {ex.Message}");
            }
        }
    }
}
