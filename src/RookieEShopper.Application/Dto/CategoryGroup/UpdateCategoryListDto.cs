using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RookieEShopper.Application.Dto.CategoryGroup
{
    public class UpdateCategoryListDto
    {
        public IList<int> CategoriesToAddId { get; set; }
        public IList<int> CategoriesToRemoveId { get; set; }
    }
}
