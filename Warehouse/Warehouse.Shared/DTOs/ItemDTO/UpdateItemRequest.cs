using System;
using System.Collections.Generic;
using System.Text;

namespace Warehouse.Shared.DTOs.ItemDTO
{
    public record UpdateItemRequest(int id,string Name, decimal? ReferencePricePerItem, string? Description);
}
