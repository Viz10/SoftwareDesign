using System.ComponentModel.DataAnnotations;

namespace Warehouse.Data.DTOs.ItemDTOs
{
    public class ItemCreateDTO /// form related data with checkings
    {
        [Required(ErrorMessage = "Name is required")]
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Price is required")]
        public decimal? PricePerItem { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }
    }
}
