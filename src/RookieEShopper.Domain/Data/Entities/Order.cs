﻿using Microsoft.EntityFrameworkCore;

namespace RookieEShopper.Domain.Data.Entities
{
    public enum OrderStatus
    {
        Processing,
        Shipping,
        Delivered
    }

    public class Order
    {
        public int Id { get; set; }
        public Customer? Customer { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public OrderStatus Status { get; set; } = OrderStatus.Processing;

        [Precision(18, 2)]
        public decimal TotalPrice { get; set; }

        public IList<OrderDetail> Detail { get; set; }
    }
}