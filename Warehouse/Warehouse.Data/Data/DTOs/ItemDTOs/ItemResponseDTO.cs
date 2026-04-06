using System.ComponentModel.DataAnnotations;

namespace Warehouse.Data.DTOs.ItemDTOs
{
        public class ItemResponseDTO
        {
            public int Id { get; set; }
            public string Name { get; set; } = null!;
            public decimal? PricePerItem { get; set; }
            public string? Description { get; set; }
        }
}
