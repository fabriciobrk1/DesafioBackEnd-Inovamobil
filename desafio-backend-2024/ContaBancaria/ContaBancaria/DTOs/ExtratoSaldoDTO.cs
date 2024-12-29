using ContaBancaria.Models;

namespace ContaBancaria.DTOs
{
    public class ExtratoSaldoDTO
    {
        public int ContaId { get; set; }

        public decimal Saldo { get; set; }
        public List<TransacaoDTO> Extrato {  get; set; }
    }
}
