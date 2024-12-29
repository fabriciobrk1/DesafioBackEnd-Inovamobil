namespace ContaBancaria.DTOs
{
    public class CriarContaDTO
    {
        public string Cnpj { get; set; }
        public string NumeroConta { get; set; }
        public string Agencia { get; set; }
        public IFormFile ImagemDocumento { get; set; }
    }
}
