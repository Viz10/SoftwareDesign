using System;
using System.Collections.Generic;
using System.Text;

namespace Warehouse.Shared.DTOs.ItemDTO
{
    public record AddItemRequest
    {
        public string Name { get; set; } = string.Empty;
        public decimal? ReferencePricePerItem { get; set; }
        public string? Description { get; set; }
    }
}
