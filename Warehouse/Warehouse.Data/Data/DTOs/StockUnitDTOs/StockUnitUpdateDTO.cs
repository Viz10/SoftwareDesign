using System.ComponentModel.DataAnnotations;
using Warehouse.Data.Entities;

namespace Warehouse.Data.DTOs.StockUnitDTOs
{
    public class StockUnitUpdateDTO
    {
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0!")]
        public decimal? ActualPrice { get; set; }
        public string? Note { get; set; }
        public string Status { get; set; } = null!;

        public DateTimeOffset ModifiedTime { get; set; } = DateTimeOffset.UtcNow;
    }
}
