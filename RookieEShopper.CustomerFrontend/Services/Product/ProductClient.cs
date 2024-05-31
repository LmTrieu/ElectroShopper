using Newtonsoft.Json;
using RookieEShopper.Api.Dto;
using RookieEShopper.CustomerFrontend.Models.Dto;
using RookieEShopper.SharedLibrary.ViewModels;
using System.Net.Http;
using System.Text;

namespace RookieEShopper.CustomerFrontend.Services.Product
{
    public class ProductClient : IProductClient
    {
        //private readonly HttpClient _httpClient;
        //public ProductClient(HttpClient httpClient)
        //{
        //    _httpClient = httpClient;

        //    _httpClient.BaseAddress = new Uri("https://localhost:7265");
        //}

        private readonly IHttpClientFactory _httpClientFactory;

        public ProductClient(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
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
            var client = _httpClientFactory.CreateClient("apiClient");

            using HttpResponseMessage response = await client
                .PostAsync("/api/Review", jsonContent);

            response.EnsureSuccessStatusCode();
        }

        public async Task<ICollection<ProductReviewVM>?> GetProductReviewsAsync(int productId)
        {
            var client = _httpClientFactory.CreateClient("apiClient");

            var response = await client.GetAsync("/api/Review/Product/" + productId);

            response.EnsureSuccessStatusCode();

            string content = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<ApiListObjectResponse<ProductReviewVM>>(content)?.Data;
        }

        public async Task<ProductVM?> GetProductDetailById(int productId)
        {
            var client = _httpClientFactory.CreateClient("apiClient");

            var response = await client.GetAsync("/api/Products/Detail/" + productId);

            response.EnsureSuccessStatusCode();

            string content = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<ApiSingleObjectResponse<ProductVM>>(content)?.Data;
        }

        public async Task<ICollection<ProductVM>?> GetProductsAsync()
        {
            var client = _httpClientFactory.CreateClient("apiClient");

            var response = await client.GetAsync("/api/Products");

            response.EnsureSuccessStatusCode();

            string content = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<ApiListObjectResponse<ProductVM>>(content)?.Data;
        }

        public async Task<ICollection<ProductVM>?> GetProductsByCategoryAsync(int categoryId)
        {
            var client = _httpClientFactory.CreateClient("apiClient");

            var response = await client.GetAsync("/api/Products/Category/" + categoryId);

            response.EnsureSuccessStatusCode();

            string content = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<ApiListObjectResponse<ProductVM>>(content)?.Data;
        }
    }
}
