using System.ComponentModel.DataAnnotations;

namespace InventoryService.Infrastructure.Entities
{
    public enum UnitStatus { Available, Reserved, Shipped, Returned }

    public class StockUnit : ISoftDeletable
    {
        [Key]
        public int Id { get; set; }


        public int ItemId { get; set; }
        public Item Item { get; set; } = null!; // FK


        public string SerialNumber { get; set; } = null!;
        public string? Note { get; set; }
        public decimal? CurrentPrice { get; set; }


        public DateTimeOffset CreatedAtTime { get; set; }
        public DateTimeOffset? LastModifiedTime { get; set; }
        public bool IsDeleted { get; set; } = false;


        public UnitStatus Status { get; set; } = UnitStatus.Available;
        //public List<OrderLine> OrderLines { get; set; } = new List<OrderLine>();
    }

}
