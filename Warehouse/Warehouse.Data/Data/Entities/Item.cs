using Microsoft.EntityFrameworkCore.Query.Internal;
using System.ComponentModel.DataAnnotations;

namespace Warehouse.Data.Entities
{
    public class Item : IEntity /// Samsung Galaxy S26
    {
        [Key]
        public int Id { get; set; }
        
        public string Name { get; set; } = null!;
        public decimal? PricePerItem { get; set; } /// reference price
       
        public string? Description { get; set; }
        
        public DateTimeOffset? DeletedAtTime {  get; set; }
        public DateTimeOffset LastModifiedTime { get; set; }

        public bool IsDeleted { get; set; } = false;

        public List<StockUnit> StockUnits { get; set; } = new List<StockUnit>();
        public List<Stock> Stocks { get; set; } = new List<Stock>();
    }
}
