using System;
using System.Collections.Generic;
using System.Text;

namespace Warehouse.Shared.DTOs.ItemDTO
{
    public record ItemGetResponse /// init used to construct record only
    {
        public int Id { get; init; }
        public string Name { get; init; } = null!;
        public decimal? ReferencePricePerItem { get; init; }
        public string? Description { get; init; }
        public int Quantity { get; init; }
    }
}
