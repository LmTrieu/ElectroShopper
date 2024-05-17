namespace RookieEShopper.Domain.Data.Entities
{
    public class ProductCategory
    {
        public int Id { get; set; }
        public Category category { get; set; }
        public Product Product { get; set; }
    }
}