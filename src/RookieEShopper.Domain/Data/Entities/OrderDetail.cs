using RookieEShopper.Domain.Data.Entities;

namespace RookieEShopper.Domain.Data.Entities
{
    public class OrderDetail
    {
        public int Id { get; set; }
        public required Order Order { get; set; }
        public required ICollection<ProductOrder> Products{ get; set; }
    }
}
