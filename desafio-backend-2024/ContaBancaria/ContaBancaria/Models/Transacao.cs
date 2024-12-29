using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ContaBancaria.Models
{
    public class Transacao
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public decimal Valor { get; set; }

        [Required]
        public string Tipo { get; set; } 

        [Required]
        [ForeignKey("Conta")]
        public int ContaId { get; set; }

        public Conta Conta { get; set; }

        [Required]
        public DateTime Data {  get; set; }
    }
}
