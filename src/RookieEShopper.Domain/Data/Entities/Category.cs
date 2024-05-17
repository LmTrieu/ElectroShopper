namespace RookieEShopper.Domain.Data.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string CartegoryName { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}