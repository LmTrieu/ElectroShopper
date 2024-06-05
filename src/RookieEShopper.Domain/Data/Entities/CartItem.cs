namespace RookieEShopper.Domain.Data.Entities
{
    public class CartItem
    {
        public int Id { get; set; }
        public int Quantity { get; set; }

        public Cart? Cart { get; set; }
        public Product? Product { get; set; }
        public Coupon? AppliedCoupon { get; set; }
    }
}