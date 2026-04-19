using Microsoft.EntityFrameworkCore.Query.Internal;
using System.ComponentModel.DataAnnotations;
using Warehouse.Data.Data.Entities;

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


        public Stock Stock { get; set; } = new Stock();
        public List<StockUnit> StockUnits { get; set; } = new List<StockUnit>();
        public List<OrderLine> OrderLines { get; set; } = new List<OrderLine>();   
    }
}
