
namespace Warehouse.Data.Entities
{
    public interface IEntity
    {
        int Id { get; set; }
        public DateTimeOffset? DeletedAtTime { get; set; }
        public DateTimeOffset LastModifiedTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}
