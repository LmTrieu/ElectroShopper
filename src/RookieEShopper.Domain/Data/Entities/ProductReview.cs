using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RookieEShopper.Domain.Data.Entities
{
    public class ProductReview
    {
        public int Id { get; set; }
        public required Customer Customer { get; set; }
        public required Product Product { get; set; }
        public string Feedback { get; set; } = string.Empty;
        public int Rating { get; set; } = 0;
    }
}
