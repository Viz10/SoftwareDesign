using System;
using System.Collections.Generic;
using System.Text;

namespace Warehouse.Shared.Common
{
    public class WarehouseEvent
    {
        public string EntityType { get; set; } = null!;
        public string AccountEmail { get; set; } = null!;
        public string Action { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTimeOffset OccurredAt { get; set; } = DateTimeOffset.UtcNow;
    }
}
