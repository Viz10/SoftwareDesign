using System.ComponentModel.DataAnnotations;

namespace Warehouse.Data.Entities
{

    public enum UnitStatus { Available, Reserved, Shipped, Returned }

    public class StockUnit
    {
        [Key]
        public int Id { get; set; } 

        [Required]
        public int ItemId { get; set; } 
        public Item Item { get; set; } = null!;

        public string? SerialNumber { get; set; }
        public string? Note { get; set; }

        public UnitStatus Status { get; set; } = UnitStatus.Available;
    }
}
