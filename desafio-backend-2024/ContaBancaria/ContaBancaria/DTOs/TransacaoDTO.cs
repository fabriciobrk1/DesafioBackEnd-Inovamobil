namespace ContaBancaria.DTOs
{
    public class TransacaoDTO
    {
        public decimal Valor { get; set; }
        public string Tipo { get; set; }
        public int ContaId { get; set; }
        public DateTime Data {  get; set; }
    }
}
