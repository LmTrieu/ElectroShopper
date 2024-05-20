﻿using Microsoft.EntityFrameworkCore;

namespace RookieEShopper.Domain.Data.Entities
{
    public class Customer
    {
        public int Id { get; set; }
        public ICollection<Order> OrderHistory { get; set; } = new List<Order>();

        [Precision(18, 2)]
        public decimal EWallet { get; set; } = 0;
        public Cart? ShoppingCart { get; set; } 
    }
}