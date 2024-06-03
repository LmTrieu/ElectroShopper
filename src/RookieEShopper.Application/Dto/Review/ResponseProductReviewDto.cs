using RookieEShopper.Application.Dto.Customer;
using RookieEShopper.Application.Dto.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RookieEShopper.Application.Dto.Review
{
    public class ResponseProductReviewDto
    {
        public int Id { get; set; }
        public ResponseCustomerDto? Customer {  get; set; }
        public int Rating { get; set; }
        public string Feedback { get; set; } = string.Empty;
    }
}
