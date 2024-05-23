namespace RookieEShopper.SharedViewModel
{
    public class ProductVM
    {
        public int id { get; set; }
        public string name { get; set; }
        public decimal price { get; set; }
        public string mainImagePath { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public IList<string> imageGallery = new List<string>();
        public CategoryVM category { get; set; } = new CategoryVM();
    }
}