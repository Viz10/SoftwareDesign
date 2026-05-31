
using Warehouse.Shared.Enums;

namespace Warehouse.Shared.DTOs.StockUnitDTO
{
    public record UpdateStockUnitRequest
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public decimal? CurrentPrice { get; set; }
        public string? Note { get; set; }
        public UnitStatus Status { get; set; }
    }
}
