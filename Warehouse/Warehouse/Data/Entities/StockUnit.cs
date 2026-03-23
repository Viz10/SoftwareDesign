using System.ComponentModel.DataAnnotations;

namespace Warehouse.Data.Entities
{
    public enum UnitStatus { Available, Reserved, Shipped, Returned }

    public class StockUnit : IEntity /// Samsung Galaxy S26 
    {
        [Key]
        public int Id { get; set; } 

        public int ItemId { get; set; } 
        public Item Item { get; set; } = null!;

        public string SerialNumber { get; set; } = null!;
        public string? Note { get; set; }

        public decimal? ActualPrice { get; set; }

        public DateTimeOffset? DeletedAtTime { get; set; }
        public DateTimeOffset LastModifiedTime { get; set; }

        public bool IsDeleted { get; set; } = false;
        public UnitStatus Status { get; set; } = UnitStatus.Available;
    }
}
