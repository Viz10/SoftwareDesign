using Warehouse.Data.Entities;

namespace Warehouse.Data.Data.DTOs.StockUnitDTOs
{
        public class StockUnitGetDTO
        {
            public int Id { get; set; }
            public string Name { get; set; } = null!; /// from Item
            public string SerialNumber { get; set; } = null!;
            public string? Note { get; set; }
            public decimal? CurrentPrice { get; set; }
            public UnitStatus Status { get; set; }
        }

}
