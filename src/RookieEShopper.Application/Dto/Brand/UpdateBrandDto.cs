using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RookieEShopper.Application.Dto.Brand
{
    public class UpdateBrandDto
    {
        public string? Name { get; set; }
        public int[]? CategoriesId { get; set; }
    }
}
