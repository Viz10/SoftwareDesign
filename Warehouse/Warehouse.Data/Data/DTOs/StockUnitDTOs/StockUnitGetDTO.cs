using Warehouse.Data.Entities;

namespace Warehouse.Data.DTOs.StockUnitDTOs
{
        public class StockUnitGetDTO
        {
            public int Id { get; set; }
            public string Name { get; set; } = null!; /// from Item
            public decimal? ActualPrice { get; set; }
            public string? Note { get; set; }
            public string SerialNumber  { get; set; } = null!;
            public UnitStatus Status { get; set; }
    }

}
