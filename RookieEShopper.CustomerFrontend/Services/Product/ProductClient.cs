using Newtonsoft.Json;
using RookieEShopper.CustomerFrontend.Models;
using RookieEShopper.SharedViewModel;
using System.Net.Http;
using System.Text;

namespace RookieEShopper.CustomerFrontend.Services.Product
{
    public class ProductClient : IProductClient
    {
        private readonly HttpClient _httpClient;
        public ProductClient(HttpClient httpClient)
        {
            _httpClient = httpClient;

            _httpClient.BaseAddress = new Uri("https://localhost:7265");
        }

        public async Task PostProductReviewAsync(CreateProductReviewDto createProductReviewVM)
        {
            using StringContent jsonContent = new(
                JsonConvert.SerializeObject(new
                {
                    productId = createProductReviewVM.ProductId,
                    customerId = createProductReviewVM.CustomerId,
                    feedback = createProductReviewVM.Feedback,
                    rating = createProductReviewVM.Rating
                }),
                Encoding.UTF8,"application/json");

            using HttpResponseMessage response = await _httpClient
                .PostAsync("/api/Review", jsonContent);

            response.EnsureSuccessStatusCode();
        }

        public async Task<ICollection<ProductReviewVM>?> GetProductReviewsAsync(int productId)
        {
            var response = await _httpClient.GetAsync("/api/Review/Product/" + productId);

            response.EnsureSuccessStatusCode();

            string content = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<ICollection<ProductReviewVM>>(content);
        }

        public async Task<ProductVM?> GetProductDetailById(int productId)
        {
            var response = await _httpClient.GetAsync("/api/Products/" + productId);

            response.EnsureSuccessStatusCode();

            string content = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<ProductVM>(content);
        }

        public async Task<ICollection<ProductVM>?> GetProductsAsync()
        {
            var response = await _httpClient.GetAsync("/api/Products");

            response.EnsureSuccessStatusCode();

            string content = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<IList<ProductVM>>(content);
        }

        public async Task<ICollection<ProductVM>?> GetProductsByCategoryAsync(int categoryId)
        {
            var response = await _httpClient.GetAsync("/api/Products/Category/" + categoryId);

            response.EnsureSuccessStatusCode();

            string content = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<IList<ProductVM>>(content);
        }
    }
}
