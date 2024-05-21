using RookieEShopper.Domain.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RookieEShopper.Application.Dto.CategoryGroup
{
    public class CreateCategoryGroupDto
    {
        public string Name {  get; set; }
        public string Description { get; set; }
        public IList<int>? CategoriesId { get; set; }
    }
}
