using ContaBancaria.DTOs;
using ContaBancaria.Services;
using Microsoft.AspNetCore.Mvc;

namespace ContaBancaria.Controllers
{
    public class TransferirController : ControllerBase
    {

        private readonly ContaService _contaService;


        public TransferirController(ContaService contaService)
        {
            _contaService = contaService;
        }

        [HttpPost("transferir")]
        public async Task<IActionResult> Transferir([FromBody] TransacaoDTO transacaoDTO, int contaDestinoId)
        {
            if (transacaoDTO.Data == DateTime.MinValue)
                transacaoDTO.Data = DateTime.Now; 

            var resultado = await _contaService.TransferirAsync(transacaoDTO, contaDestinoId);

            if (resultado)
                return Ok("Transferência realizada com sucesso.");

            return BadRequest("Erro ao realizar a transferência.");
        }
    }
}