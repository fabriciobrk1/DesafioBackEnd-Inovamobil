using System.ComponentModel.DataAnnotations;

namespace ContaBancaria.DTOs
{
    public class SaqueDTO
    {
        public int IdConta { get; set; }

        
        [Required(ErrorMessage = "O campo valor é obrigatório.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "O valor deve ser maior que zero.")]
        public decimal Valor { get; set; }
    }
}
