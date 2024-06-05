namespace RookieEShopper.SharedLibrary.ViewModels
{
    public class ProductDetailPageVM
    {
        public ProductVM Product { get; set; }
        public ICollection<ProductReviewVM> Reviews { get; set; }
    }
}