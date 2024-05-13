using Microsoft.EntityFrameworkCore;

namespace ElectroShopper.Data.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [Precision(18, 2)]
        public decimal Price { get; set; } = 0;

        public virtual Category Category { get; set; }

    }
}
