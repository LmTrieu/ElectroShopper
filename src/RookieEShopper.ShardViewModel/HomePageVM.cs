namespace RookieEShopper.SharedViewModel
{
    public class HomePageVM
    {
        public IList<CategoryVM> Catergories { get; set; } = new List<CategoryVM>();
        public IList<ProductVM> Products { get; set; } = new List<ProductVM>();
    }
}