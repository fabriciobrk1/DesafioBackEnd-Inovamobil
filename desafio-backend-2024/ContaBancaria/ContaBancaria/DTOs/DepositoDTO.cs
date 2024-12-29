using System.ComponentModel.DataAnnotations;

namespace ContaBancaria.DTOs
{
    public class DepositoDTO
    {
        public int IdConta { get; set; }

        [Required(ErrorMessage = "O campo é obrigatório.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "O valor deve ser maior que zero.")]
        public decimal Valor { get; set; }
    }
}
