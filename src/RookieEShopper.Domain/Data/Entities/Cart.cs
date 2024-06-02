using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RookieEShopper.Domain.Data.Entities
{
    public class Cart
    {
        public int Id { get; set; }
        public Guid CustomerId { get; set; }
        [Precision(18,2)]
        public decimal Total { get; set; }
        public List<CartItem> Items { get; set; } = new List<CartItem>();
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
