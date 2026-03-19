using System.ComponentModel.DataAnnotations;

namespace Warehouse.Data.DTOs.ItemDTOs
{
    public class ItemUpdateDTO
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
