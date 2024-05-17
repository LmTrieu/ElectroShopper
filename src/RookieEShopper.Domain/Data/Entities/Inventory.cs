namespace RookieEShopper.Domain.Data.Entities
{
    public class Inventory
    {
        public int Id { get; set; }
        public Product Product { get; set; }
        public int StockAmmount { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}