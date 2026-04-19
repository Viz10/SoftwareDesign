using System;
using System.Collections.Generic;
using System.Text;

namespace Warehouse.Services.Services.Events
{
    public class WarehouseEventBus
    {
        public event Action<WarehouseEvent>? OnEvent; /// the multi delegate when called calls all registered functions

        public void Publish(WarehouseEvent warehouseEvent) /// notifies children
        {
            OnEvent?.Invoke(warehouseEvent);
        }
    }
}
