namespace RookieEShopper.Domain.Data.Entities
{
    public class Brand
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsLocked { get; set; }
        public IList<Category>? Categories { get; set; }
    }
}