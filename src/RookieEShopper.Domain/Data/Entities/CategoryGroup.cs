using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RookieEShopper.Domain.Data.Entities
{
    public class CategoryGroup
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IList<Category> Categories { get; set;}
        public string Description { get; set; }
    }
}
