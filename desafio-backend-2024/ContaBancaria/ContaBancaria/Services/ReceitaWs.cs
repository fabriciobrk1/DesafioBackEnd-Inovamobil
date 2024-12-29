using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace ContaBancaria.Services
{
    public class ReceitaWs
    {
        private readonly HttpClient _httpClient;

        public ReceitaWs(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string?> ObterNomeEmpresaPorCnpj(string cnpj)
        {
            string url = $"https://receitaws.com.br/v1/cnpj/{cnpj}";

            try
            {
                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                    throw new HttpRequestException($"Erro ao acessar ReceitaWS: {response.ReasonPhrase}");

                string jsonRetorno = await response.Content.ReadAsStringAsync();
                var dadosReceita = JsonConvert.DeserializeObject<ReceitaFederal>(jsonRetorno);

                if (dadosReceita != null && dadosReceita.Status == "OK")
                    return dadosReceita.Nome;
                else
                    throw new Exception($"Erro na resposta da ReceitaWS: {dadosReceita?.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao consultar CNPJ: {ex.Message}");
            }
        }
    }

    // Classe para deserializar a resposta da API
    public class ReceitaFederal
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public string Nome { get; set; }
    }
}
