using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Warehouse.Data.Entities;

namespace Warehouse.Data.Data.Entities
{
    public class OrderLine
    {
        [Key]
        public int Id { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; } = null!;

        public int ItemId { get; set; }
        public Item Item { get; set; } = null!;

        public int? StockUnitId { get; set; }
        public StockUnit? StockUnit { get; set; } /// might assign later which specific one

        public DateTimeOffset? ShippedAt { get; set; } /// an stock item from the order can be shupped later
    }
}
