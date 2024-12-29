using ContaBancaria.DTOs;
using ContaBancaria.Models;
using ContaBancaria.Services;
using Microsoft.AspNetCore.Mvc;

namespace ContaBancaria.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContaController : ControllerBase
    {
        private readonly ContaService _contaService;

        
        public ContaController(ContaService contaService)
        {
            _contaService = contaService;
        }

        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetConta(int id)
        {
            var conta = await _contaService.BuscarContaPorIdAsync(id);
            if (conta == null)
                return NotFound("Não existe nenhuma conta com esse id");

            return Ok(conta);
        }

        
        [HttpGet]
        public async Task<IActionResult> GetContas()
        {
            var contas = await _contaService.BuscarTodasContasAsync();
            if (contas == null || !contas.Any())
                return NotFound("Nenhuma conta encontrada.");

            return Ok(contas);
        }

        
        [HttpPost]
        public async Task<IActionResult> CriarConta([FromForm] CriarContaDTO contaDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var contaCriada = await _contaService.CriarContaAsync(contaDto);
                return CreatedAtAction(nameof(GetConta), new { id = contaCriada.Id }, contaCriada);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        
        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarConta(int id, [FromForm] CriarContaDTO contaDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var contaAtualizada = await _contaService.AtualizarContaAsync(id, contaDto);
                if (contaAtualizada == null)
                    return NotFound("Conta não encontrada");

                return Ok(contaAtualizada);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletarConta(int id)
        {
            try
            {
                var resultado = await _contaService.ExcluirContaAsync(id);
                if (!resultado)
                    return NotFound("Conta não encontrada");

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }
    }
}
