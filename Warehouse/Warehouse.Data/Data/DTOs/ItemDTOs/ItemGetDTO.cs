using System.ComponentModel.DataAnnotations;

namespace Warehouse.Data.Data.DTOs.ItemDTOs
{
        public class ItemGetDTO
        {
            public int Id { get; set; }
            public string Name { get; set; } = null!;
            public decimal? ReferencePricePerItem { get; set; }
            public string? Description { get; set; }
            public int Quantity { get; set; } = 0;
    }
}
