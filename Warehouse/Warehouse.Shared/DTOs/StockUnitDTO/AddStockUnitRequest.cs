
namespace Warehouse.Shared.DTOs.StockUnitDTO
{
    public record AddStockUnitRequest
    {
        public int ItemId { get; set; }
        public decimal? CurrentPrice { get; set; }
        public string? Note { get; set; }
        public int Quantity { get; set; } = 0;
    }
}
