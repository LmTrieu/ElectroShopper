
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using RookieEShopper.SharedViewModel;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RookieEShopper.CustomerFrontend.Services.Category
{
    public class CategoryClient : ICategoryClient
    {
        private readonly HttpClient _httpClient;

        public CategoryClient(HttpClient httpClient)
        {
            _httpClient = httpClient;

            _httpClient.BaseAddress = new Uri("https://localhost:7265");
        }
        public async Task<ICollection<CategoryGroupVM>?> GetCategoriesAsync()
        {
            var response = await _httpClient.GetAsync("/api/Categories/CategoryGroup");

            response.EnsureSuccessStatusCode();

            string content = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<IList<CategoryGroupVM>>(content);
        }

    }
}
