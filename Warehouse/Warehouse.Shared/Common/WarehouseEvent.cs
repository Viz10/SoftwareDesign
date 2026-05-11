using System;
using System.Collections.Generic;
using System.Text;

namespace Warehouse.Shared.Common
{
    public class WarehouseEvent
    {
        public string EntityType { get; set; } = "";
        public string AccountEmail { get; set; } = "";
        public string Action { get; set; } = "";
        public string Description { get; set; } = "";
        public DateTimeOffset OccurredAt { get; set; } = DateTimeOffset.UtcNow;
    }
}
