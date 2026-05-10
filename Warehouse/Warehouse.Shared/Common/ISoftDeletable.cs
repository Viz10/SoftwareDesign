using System;
using System.Collections.Generic;
using System.Text;

namespace Warehouse.Shared.Common
{
    public interface ISoftDeletable
    {
        int Id { get; set; }
        public DateTimeOffset CreatedAtTime { get; set; }
        public DateTimeOffset? LastModifiedTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}
