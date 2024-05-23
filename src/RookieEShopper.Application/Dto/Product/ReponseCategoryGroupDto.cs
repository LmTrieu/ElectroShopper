using RookieEShopper.Application.Dto.Category;
using RookieEShopper.SharedViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RookieEShopper.Application.Dto.Product
{
    public class ReponseCategoryGroupDto
    {
        public int id { get; set; }
        public string name { get; set; }
        public IList<ResponseCategoryDto>? categories { get; set; }
    }
}
