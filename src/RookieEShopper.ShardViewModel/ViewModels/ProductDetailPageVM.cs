using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RookieEShopper.SharedLibrary.ViewModels
{
    public class ProductDetailPageVM
    {
        public ProductVM Product { get; set; }
        public ICollection<ProductReviewVM> Reviews { get; set; }
    }
}
