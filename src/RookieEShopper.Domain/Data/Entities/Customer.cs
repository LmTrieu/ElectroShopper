using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RookieEShopper.Domain.Data.Entities
{
    public class Customer
    {
        public int Id { get; set; }      
        public ICollection<Order> OrderHistory { get; set; } = new List<Order>();
        [Precision(18, 2)]
        public decimal EWallet {  get; set; } = 0;
    }
}
