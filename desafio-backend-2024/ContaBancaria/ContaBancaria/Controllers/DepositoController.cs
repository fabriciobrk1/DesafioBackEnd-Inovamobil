using ContaBancaria.DTOs;
using ContaBancaria.Services;
using Microsoft.AspNetCore.Mvc;

namespace ContaBancaria.Controllers
{
  
        public class DepositoController : ControllerBase
        {
            private readonly ContaService _contaService;


            public DepositoController(ContaService contaService)
            {
                _contaService = contaService;
            }

            [HttpPost("depositar")]
            public async Task<IActionResult> Depositar([FromBody] DepositoDTO depositoDto)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                try
                {
                    var contaAtualizada = await _contaService.DepositarAsync(depositoDto.IdConta, depositoDto.Valor);
                    return Ok(new
                    {
                        mensagem = "Depósito realizado com sucesso.",
                        saldoAtual = contaAtualizada.Saldo
                    });
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Erro interno: {ex.Message}");
                }
            }
        }
    }
