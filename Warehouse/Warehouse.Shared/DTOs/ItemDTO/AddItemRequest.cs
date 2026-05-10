using System;
using System.Collections.Generic;
using System.Text;

namespace Warehouse.Shared.DTOs.ItemDTO
{
    public record AddItemRequest(string Name, decimal? ReferencePricePerItem, string? Description);
}
