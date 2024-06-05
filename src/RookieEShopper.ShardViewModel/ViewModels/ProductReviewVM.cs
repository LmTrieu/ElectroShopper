namespace RookieEShopper.SharedLibrary.ViewModels
{
    public class ProductReviewVM
    {
        public int Id { get; set; }
        public string Feedback { get; set; } = string.Empty;
        public CustomerVM Customer { get; set; }
        public int Rating { get; set; }
    }
}