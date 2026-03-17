using Microsoft.EntityFrameworkCore.Query.Internal;
using System.ComponentModel.DataAnnotations;

namespace Warehouse.Data.Entities
{
    public class Item
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public decimal PricePerItem { get; set; }

        public DateTimeOffset? DeletedAtTime {  get; set; }
        public DateTimeOffset LastModifiedTime { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}
