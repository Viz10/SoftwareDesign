using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Warehouse.Data.Entities;

namespace Warehouse.Data.Data.Entities
{

    public enum OrderStatus
    {
        Pending,
        Shipped,
        Delivered,
        Cancelled
    }
    public class Order
    {
        [Key]
        public int Id { get; set; }

        public int CustomerId { get; set; }
        public Account Customer { get; set; } = null!;

        public int SellerId { get; set; }
        public Account Seller { get; set; } = null!;

        public DateTimeOffset CreatedAt { get; set; }
        public OrderStatus Status { get; set; }

        public List<OrderLine> OrderLines { get; set; } = new List<OrderLine>();
    }
}
