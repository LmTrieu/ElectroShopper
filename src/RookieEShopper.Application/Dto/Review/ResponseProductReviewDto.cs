using RookieEShopper.Application.Dto.Customer;

namespace RookieEShopper.Application.Dto.Review
{
    public class ResponseProductReviewDto
    {
        public int Id { get; set; }
        public ResponseCustomerDto? Customer { get; set; }
        public int Rating { get; set; }
        public string Feedback { get; set; } = string.Empty;
    }
}