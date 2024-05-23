using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RookieEShopper.Application.Dto.Order
{
    public class CreateOrderVM
    {
        public int CustomerId { get; set; }
        public ICollection<int> ProductIds { get; set; }
        
    }
}
