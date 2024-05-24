namespace RookieEShopper.SharedViewModel
{
    public class ProductVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string MainImagePath { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public IList<string> ImageGallery = new List<string>();
        public CategoryVM Category { get; set; } = new CategoryVM();
    }
}