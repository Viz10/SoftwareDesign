using System.ComponentModel.DataAnnotations;

namespace InventoryService.Infrastructure.Entities
{
    public class Item : ISoftDeletable
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; } = null!;
        public decimal? ReferencePricePerItem { get; set; }
        public string? Description { get; set; }

        public DateTimeOffset CreatedAtTime { get; set; }
        public DateTimeOffset? LastModifiedTime { get; set; }
        public bool IsDeleted { get; set; } = false;


        public Stock? Stock { get; set; }
        public List<StockUnit> StockUnits { get; set; } = new List<StockUnit>();
        //public List<OrderLine> OrderLines { get; set; } = new List<OrderLine>();
    }

}
