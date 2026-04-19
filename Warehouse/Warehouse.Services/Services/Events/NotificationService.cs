using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Warehouse.Services.Services.Events
{
    public class NotificationService : IDisposable
    {
        private readonly WarehouseEventBus bus;

        public NotificationService(WarehouseEventBus _bus)
        {
            bus = _bus;
            bus.OnEvent += HandleEvent;/// subscribe
        }

        private void HandleEvent(WarehouseEvent e)
        {

            _ = SendEmailAsync(e);
        }

        private async Task SendEmailAsync(WarehouseEvent e)
        {
            try
            {
                string logEntry = $@"
                                --------------------------------------------------
                                Action:    {e.Action}
                                Entity:    {e.EntityType}
                                Details:   {e.Description}
                                Occurred:  {e.OccurredAt}
                                --------------------------------------------------";

                string sourcePath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Report.txt"));
                using (StreamWriter writer = new StreamWriter(sourcePath, true))
                {
                    await writer.WriteLineAsync(logEntry);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void Dispose()
        {
            bus.OnEvent -= HandleEvent;
        }
    }
}
