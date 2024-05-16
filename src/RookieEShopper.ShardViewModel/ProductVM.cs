using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RookieEShopper.SharedViewModel
{
    public class ProductVM
    {
        public int id { get; set; }
        public string name { get; set; }
        public decimal price { get; set; }
        public CategoryVM category { get; set; } = new CategoryVM();
    }
}
