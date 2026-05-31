
namespace Warehouse.Shared.DTOs.ItemDTO
{
    public record ItemGetResponse /// init used to construct record only once ,by mapper with prop. accesor
    {
        public int Id { get; init; }
        public string Name { get; init; } = null!;
        public decimal? ReferencePricePerItem { get; init; }
        public string? Description { get; init; }
        public int Quantity { get; init; }
    }
}
