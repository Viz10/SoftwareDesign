using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Warehouse.Data.Data.Entities
{
    public class Receipt /// Receipts are completely immutable once generated
    {
        [Key]
        public int Id { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; } = null!;

        public DateTimeOffset GeneratedAt { get; set; }


        ///  snapshot
        public string BuyerEmail { get; set; } = null!;
        public string BuyerFullName { get; set; } = null!;
        public string ShippingAddress { get; set; } = null!;

        public decimal Subtotal { get; set; }
        public decimal Tax { get; set; }
        public decimal Total { get; set; }

        public ICollection<ReceiptLine> Lines { get; set; } = new List<ReceiptLine>();
    }
}
