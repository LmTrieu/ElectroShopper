namespace RookieEShopper.Domain.Data.Entities
{
    public class ProductReview
    {
        public int Id { get; set; }
        public OrderDetail? OrderDetail { get; set; }
        public Customer? Customer { get; set; }
        public Product? Product { get; set; }
        public string Feedback { get; set; } = string.Empty;
        public int Rating { get; set; } = 0;
    }
}