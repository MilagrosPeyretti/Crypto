using System.Text.Json;
using System.Net.Http;

namespace APICrypto.Services
{
    public class CryptoYaService : ICryptoYaService
    {
        private readonly HttpClient _httpClient;

        public CryptoYaService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<decimal?> GetPriceAsync(string exchange, string cryptoCode)
        {
            try
            {
                var url = $"https://criptoya.com/api/{exchange}/{cryptoCode}/ars";
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                using var content = await response.Content.ReadAsStreamAsync();
                var json = await JsonDocument.ParseAsync(content);

                var price = json.RootElement.GetProperty("totalAsk").GetDecimal();
                return price;
            }
            catch
            {
                return null;
            }
        }
        public async Task<decimal?> GetPriceSumary(string cryptoCode)
        {
            try
            {
                var url = $"https://criptoya.com/api/satoshitango/{cryptoCode}/ars";
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                using var content = await response.Content.ReadAsStreamAsync();
                var json = await JsonDocument.ParseAsync(content);

                var price = json.RootElement.GetProperty("totalAsk").GetDecimal();
                return price;
            }
            catch
            {
                return null;
            }
        }
    }

   
}
