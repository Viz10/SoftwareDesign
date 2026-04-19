using System.ComponentModel.DataAnnotations;

namespace Warehouse.Data.Data.DTOs.ItemDTOs
{
    public class ItemSendDTO 
    {
        [Required(ErrorMessage = "Name is required")]
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Price is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be greater than 0!")]
        public decimal? ReferencePricePerItem { get; set; }
            
        [MaxLength(500)]
        public string? Description { get; set; }
    }
}
