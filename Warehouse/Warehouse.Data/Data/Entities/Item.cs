using Microsoft.EntityFrameworkCore.Query.Internal;
using System.ComponentModel.DataAnnotations;

namespace Warehouse.Data.Entities
{
    public class Item : IEntity
    {
        [Key]
        public int Id { get; set; }
        
        public string Name { get; set; } = null!;
        public decimal? ReferencePricePerItem { get; set; } 
        public string? Description { get; set; }
        
        public DateTimeOffset? DeletedAtTime {  get; set; }
        public DateTimeOffset LastModifiedTime { get; set; }
        public bool IsDeleted { get; set; } = false;

        public List<StockUnit> StockUnits { get; set; } = new List<StockUnit>();
        public Stock Stock { get; set; } = new Stock();
    }
}
