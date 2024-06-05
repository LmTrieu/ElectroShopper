namespace RookieEShopper.Domain.Data.Entities
{
    public class CategoryGroup
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IList<Category> Categories { get; set; }
        public string Description { get; set; }
    }
}