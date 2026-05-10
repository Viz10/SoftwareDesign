using System;
using System.Collections.Generic;
using System.Text;

namespace Warehouse.Shared.DTOs.ItemDTO
{
    public class ItemGetResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal? ReferencePricePerItem { get; set; }
        public string? Description { get; set; }
        public int Quantity { get; set; }
    }
}
