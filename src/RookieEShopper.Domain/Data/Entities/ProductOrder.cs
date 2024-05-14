using Microsoft.EntityFrameworkCore;

namespace RookieEShopper.Domain.Data.Entities
{
    public class ProductOrder
    {
        public int Id { get; set; }
        public required Product Product { get; set; }
        [Precision(18, 2)]
        public decimal Discount { get; set; } = 0;
        public string Coupon { get; set; } = string.Empty;
        [Precision(18, 2)]
        public decimal Price { get; set; }
    }
}
