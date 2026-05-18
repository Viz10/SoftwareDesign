using System;
using System.Collections.Generic;
using System.Text;

namespace Warehouse.Shared.DTOs.ItemDTO
{
    public record AddItemRequest /// looks like a class but gets record equality check fields = and toString() print all , no longer immutable
    {
        public string Name { get; set; } = string.Empty;
        public decimal? ReferencePricePerItem { get; set; }
        public string? Description { get; set; }
    }
}
