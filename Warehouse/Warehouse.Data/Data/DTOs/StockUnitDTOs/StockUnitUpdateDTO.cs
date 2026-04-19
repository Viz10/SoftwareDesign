
using System.ComponentModel.DataAnnotations;
using Warehouse.Data.Entities;

namespace Warehouse.Data.Data.DTOs.StockUnitDTOs
{
    public class StockUnitUpdateDTO
    {
        public int ItemId { get; set; }

        [Required(ErrorMessage = "Current Price needed!")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0!")]
        public decimal? CurrentPrice { get; set; }

        [MaxLength(500)]
        public string? Note { get; set; }

        public UnitStatus? Status { get; set; }
    }
}
