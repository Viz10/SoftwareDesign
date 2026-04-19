using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Warehouse.Data.Data.Entities
{
    public class ReceiptLine
    {
        [Key]
        public int Id { get; set; }

        public int ReceiptId { get; set; }
        public Receipt Receipt { get; set; } = null!;

 
        /// snapshot 
        public string ProductName { get; set; } = null!;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal LineTotal { get; set; }
    }
}
