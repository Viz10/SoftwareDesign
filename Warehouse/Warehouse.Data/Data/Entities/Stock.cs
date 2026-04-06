using System.ComponentModel.DataAnnotations;

namespace Warehouse.Data.Entities
{
    public class Stock : IEntity /// quantity table for a specific item
    {
        [Key]
        public int Id { get; set; }

        public int ItemId { get; set; }
        public Item Item { get; set; } = null!;

        public int Quantity { get; set; } = 0;
    }
}
