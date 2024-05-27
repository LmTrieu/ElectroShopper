using RookieEShopper.Application.Dto.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RookieEShopper.Application.Dto.CategoryGroup
{
    public class ResponseCategoryGroupDto
    {
        public int id { get; set; }
        public string name { get; set; }
        public IList<ResponseCategoryDto>? categories { get; set; }
    }
}
