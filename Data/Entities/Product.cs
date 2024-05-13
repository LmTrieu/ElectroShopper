using Microsoft.EntityFrameworkCore;

namespace ElectroShopper.Data.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string Description { get; set; } = string.Empty;
        [Precision(18, 2)]
        public decimal Price { get; set; } = 0;
        public virtual Category? Category { get; set; }
    }
}
