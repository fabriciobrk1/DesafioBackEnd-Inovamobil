using System.ComponentModel.DataAnnotations;

namespace ContaBancaria.Models
{
    public class Conta
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Nome { get; set; }

        [Required]
        public string CNPJ { get; set; }

        [Required]
        public string NumeroConta { get; set; }

        [Required]
        public string Agencia { get; set; }

        public decimal Saldo { get; set; }

        public string ImagemDocumento { get; set; }
    }
}
