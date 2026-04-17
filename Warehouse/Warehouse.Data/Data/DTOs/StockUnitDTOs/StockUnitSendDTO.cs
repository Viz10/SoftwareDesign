using System.ComponentModel.DataAnnotations;

namespace Warehouse.Data.DTOs.StockUnitDTOs
{
    public class StockUnitSendDTO
    {
        public int ItemId { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0!")]
        public decimal? ActualPrice { get; set; }

        [Required(ErrorMessage = "Quantity needed!")]
        [Range(1, double.MaxValue, ErrorMessage = "Quantity must be greater than 0!")]
        public int Quantity { get; set; }
    }
}