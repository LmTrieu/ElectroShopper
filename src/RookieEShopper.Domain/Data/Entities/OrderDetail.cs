using Microsoft.EntityFrameworkCore;

namespace RookieEShopper.Domain.Data.Entities
{
    public class OrderDetail
    {
        public int Id { get; set; }     
        public int Quantity { get; set; }

        public Order Order { get; set; }
        public Product? Product { get; set; }
        public Coupon AppliedCoupon { get; set; }
    }
}