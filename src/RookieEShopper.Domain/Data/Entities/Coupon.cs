using Microsoft.EntityFrameworkCore;

namespace RookieEShopper.Domain.Data.Entities
{
    public class Coupon
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        [Precision(5, 2)]
        public decimal Discount { get; set; }

        public DateTime ExpireDate { get; set; }
    }
}