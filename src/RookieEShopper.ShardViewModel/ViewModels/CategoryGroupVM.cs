using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RookieEShopper.SharedLibrary.ViewModels
{
    public class CategoryGroupVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IList<CategoryVM>? Categories { get; set; }
    }
}
