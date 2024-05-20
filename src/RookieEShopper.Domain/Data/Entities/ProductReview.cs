using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace RookieEShopper.Domain.Data.Entities
{
    public class ProductReview
    {
        public int Id { get; set; }
        public OrderDetail? OrderDetail { get; set; }
        public Customer? Customer { get; set; }
        public Product? Product { get; set; }
        public string Feedback { get; set; } = string.Empty;
        
        [Precision(5,2)]
        [Range(0,5)]
        public decimal Rating { get; set; } = 0;
    }
}