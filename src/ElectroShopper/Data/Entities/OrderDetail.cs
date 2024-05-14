namespace RookieEShopper.Backend.Data.Entities
{
    public class OrderDetail
    {
        public int Id { get; set; }
        public required Order Order { get; set; }
        public required ICollection<ProductOrder> Products{ get; set; }
    }
}
