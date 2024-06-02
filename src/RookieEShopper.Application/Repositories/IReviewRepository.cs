using RookieEShopper.Application.Dto.Review;
using RookieEShopper.Domain.Data.Entities;

namespace RookieEShopper.Application.Repositories
{
    public interface IReviewRepository
    {
        public Task<ResponseProductReviewDto?> GetReviewByIdAsync(int id);
        public Task<ICollection<ResponseProductReviewDto>?> GetAllReviewsAsync();
        public Task<ICollection<ResponseProductReviewDto>?> GetReviewsByProductAsync(int productId);
        public Task<ICollection<ResponseProductReviewDto>?> GetReviewsByCustomerAsync(Guid customerId);
        public Task<ResponseProductReviewDto?> CreateReviewAsync(CreateProductReviewDto productReviewDto);
    }
}
