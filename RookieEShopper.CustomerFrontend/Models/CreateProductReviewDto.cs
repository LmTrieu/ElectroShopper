namespace RookieEShopper.CustomerFrontend.Models
{
    public class CreateProductReviewDto
    {
        public int ProductId { get; set; }
        public int CustomerId { get; set; }
        //For anonymous users
        public string Feedback { get; set; } = string.Empty;
        public int Rating { get; set; } = 0;
    }
}
