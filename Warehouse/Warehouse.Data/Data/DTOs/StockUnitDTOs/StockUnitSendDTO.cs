using System.ComponentModel.DataAnnotations;

namespace Warehouse.Data.Data.DTOs.StockUnitDTOs
{
    public class StockUnitSendDTO : StockUnitUpdateDTO
    {
        [Required(ErrorMessage = "Quantity needed!")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0!")]
        public int Quantity { get; set; }
    }
}