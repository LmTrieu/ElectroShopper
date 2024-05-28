using System.ComponentModel.DataAnnotations.Schema;

namespace RookieEShopper.Domain.Data.Entities
{
    public class Category
    {
        public int Id { get; set; }
        [Column("CartegoryName")]
        public string Name { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}