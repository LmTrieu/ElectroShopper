namespace RookieEShopper.CustomerFrontend.Models.Dto
{
    public class CreateProductReviewDto
    {
        public int ProductId { get; set; }
        public Guid CustomerId { get; set; }
        public string Feedback { get; set; } = string.Empty;
        public int Rating { get; set; } = 0;
    }
}