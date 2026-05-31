
namespace Warehouse.Shared.DTOs.ItemDTO
{
    public record AddItemRequest /// looks like a class but gets record equality check fields = and toString() print all , no longer immutable
    {
        public string Name { get; set; } = string.Empty;
        public decimal? ReferencePricePerItem { get; set; } = 0;
        public string? Description { get; set; }
    }
}
