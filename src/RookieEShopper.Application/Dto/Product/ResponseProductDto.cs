using RookieEShopper.SharedLibrary.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RookieEShopper.Application.Dto.Product
{
    public class ResponseProductDto
    {
        public int id { get; set; }
        public string name { get; set; }
        public decimal price { get; set; }
        public string mainImagePath { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public IList<string> imageGallery = new List<string>();
        public CategoryVM category { get; set; } = new CategoryVM();
    }
}
