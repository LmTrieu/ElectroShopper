
using Microsoft.Net.Http.Headers;
using RookieEShopper.SharedViewModel;
using System.Text.Json;

namespace RookieEShopper.CustomerFrontend.Services
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
                       
            return JsonSerializer.Deserialize<IList<CategoryGroupVM>>(content);
        }

        public async Task<ICollection<CategoryGroupVM>> GetCategoryGroupsAsync()
        {
            var response = await _httpClient.GetAsync("/api/");
            return null;
        }

    }
}
