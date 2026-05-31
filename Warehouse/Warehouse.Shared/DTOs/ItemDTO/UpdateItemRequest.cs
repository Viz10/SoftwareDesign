
namespace Warehouse.Shared.DTOs.ItemDTO
{
    public record UpdateItemRequest
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal? ReferencePricePerItem { get; set; }
        public string? Description { get; set; }
    }
}
