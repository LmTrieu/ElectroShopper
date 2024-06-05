namespace RookieEShopper.Application.Dto.Order
{
    public class CreateOrderVM
    {
        public int CustomerId { get; set; }
        public ICollection<int> ProductIds { get; set; }
    }
}