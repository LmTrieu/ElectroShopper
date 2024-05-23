using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RookieEShopper.Application.Dto.Category
{
    public class ResponseCategoryDto
    {
        public int id { get; set; }
        public string cartegoryName { get; set; }
        public string? description { get; set; }
    }
}
