namespace InventoryService.Infrastructure.Entities
{
    public interface ISoftDeletable
    {
        int Id { get; set; }
        public DateTimeOffset CreatedAtTime { get; set; }
        public DateTimeOffset? LastModifiedTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}
