
using Warehouse.Shared.Enums;

namespace Warehouse.Shared.DTOs.StockUnitDTO
{
    public record StockUnitGetResponse
    {
        public  int Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string SerialNumber { get; init; } = string.Empty;
        public string? Note { get; init; }
        public decimal? CurrentPrice { get; init; }
        public UnitStatus Status { get; init; }
    }

}
