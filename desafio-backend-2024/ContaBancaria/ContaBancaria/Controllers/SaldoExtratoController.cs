using ContaBancaria.Services;
using Microsoft.AspNetCore.Mvc;

namespace ContaBancaria.Controllers
{
    public class SaldoExtratoController : ControllerBase
    {
        private readonly ContaService _contaService;

        public SaldoExtratoController(ContaService contaService) 
        {
            _contaService = contaService;
        }

        [HttpGet("{id}/saldoextrato")]
        public async Task<IActionResult> ObterSaldoEExtrato(int id, [FromQuery] DateTime? dataInicio, [FromQuery] DateTime? dataFim)
        {
            try
            {
                var saldoExtrato = await _contaService.ObterSaldoEExtratoAsync(id, dataInicio, dataFim);
                return Ok(saldoExtrato);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro: {ex.Message}");
            }
        }
    }
}
