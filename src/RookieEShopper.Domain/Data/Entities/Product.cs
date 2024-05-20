using Microsoft.EntityFrameworkCore;

namespace RookieEShopper.Domain.Data.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; } = string.Empty;

        [Precision(18, 2)]
        public decimal Price { get; set; } = 0;

        public string MainImagePath { get; set; } = string.Empty;
        public IList<Coupon> AppliableCoupons { get; set; } = new List<Coupon>();
        public IList<string> ImageGallery { get; set; } = new List<string>();

        public Brand? Brand { get; set; }
        public Category? Category { get; set; }
    }
}