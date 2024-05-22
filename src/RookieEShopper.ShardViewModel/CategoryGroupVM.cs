using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RookieEShopper.SharedViewModel
{
    public class CategoryGroupVM
    {
        public int id { get; set; }
        public string name { get; set; }
        public IList<CategoryVM>? categories { get; set; }
    }
}
