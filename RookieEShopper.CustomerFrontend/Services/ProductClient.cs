using RookieEShopper.SharedViewModel;
using System.Net.Http;
using System.Text.Json;

namespace RookieEShopper.CustomerFrontend.Services
{
    public class ProductClient : IProductClient
    {
        private readonly HttpClient _httpClient;
        public ProductClient(HttpClient httpClient)
        {
            _httpClient = httpClient;

            _httpClient.BaseAddress = new Uri("https://localhost:7265");
        }
        public async Task<ICollection<ProductVM>?> GetProductsAsync()
        {
            var response = await _httpClient.GetAsync("/api/Products");

            response.EnsureSuccessStatusCode();

            string content = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<IList<ProductVM>>(content);
        }
    }
}
