namespace ContaBancaria.DTOs
{
    public class ContaDTO
    {
        public string CNPJ {  get; set; }
        public string NumeroConta { get; set; }
        public string Agencia { get; set; }
        public IFormFile ImagemDocumento { get; set; }
    }
}
