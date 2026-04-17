using System.ComponentModel.DataAnnotations;

namespace Warehouse.Data.Entities
{
    public class Stock : IEntity /// quantity table for a specific item
    {
        [Key]
        public int Id { get; set; }

        public int ItemId { get; set; } /// FK
        public Item Item { get; set; } = null!;

        public int Quantity { get; set; } = 0;

        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset LastModifiedTime { get; set; }
        public DateTimeOffset? DeletedAtTime { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
