using Microsoft.EntityFrameworkCore;

namespace RookieEShopper.Domain.Data.Entities
{
    public class OrderDetail
    {
        public int Id { get; set; }
        public bool IsRefundable { get; set; } = false;

        [Precision(18, 2)]
        public decimal ShippingPrice { get; set; } = 0;

        public Order Order { get; set; }
        public Product? Product { get; set; }
    }
}