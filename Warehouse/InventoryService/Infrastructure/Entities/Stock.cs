using System.ComponentModel.DataAnnotations;

namespace InventoryService.Infrastructure.Entities
{
    public class Stock : ISoftDeletable /// quantity table for a specific item
    {
        [Key]
        public int Id { get; set; }

        public int ItemId { get; set; } /// FK
        public Item Item { get; set; } = null!;

        public int Quantity { get; set; } = 0;

        public DateTimeOffset CreatedAtTime { get; set; }
        public DateTimeOffset? LastModifiedTime { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
