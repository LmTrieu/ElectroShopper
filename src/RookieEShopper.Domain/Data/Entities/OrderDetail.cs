using Microsoft.EntityFrameworkCore;
using RookieEShopper.Domain.Data.Entities;

namespace RookieEShopper.Domain.Data.Entities
{
    public class OrderDetail
    {
        public int Id { get; set; }
        public bool IsRefundable { get; set; } = false;
        [Precision(18, 2)]
        public decimal ShippingPrice { get; set; } = 0;

        public required Order Order { get; set; }
        public ICollection<ProductOrder>? Products{ get; set; }
    }
}
