using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using Warehouse.Data.Data.Entities;
using Warehouse.Data.DbRepository;

namespace Warehouse.Services.Services.Events
{
    public class NotificationService : IDisposable
    {
        private readonly WarehouseEventBus bus;
        private readonly IDbContextFactory<WarehouseDbContext> contextFactory;

        public NotificationService(WarehouseEventBus _bus,IDbContextFactory<WarehouseDbContext> _contextFactory)
        {
            bus = _bus;
            bus.OnEvent += HandleEvent; /// subscribe
            contextFactory = _contextFactory;
        }

        private void HandleEvent(WarehouseEvent e)
        {
            _ = SendEmailAsync(e);
        }
        private async Task SendEmailAsync(WarehouseEvent e)
        {
            try
            {
                using var db = contextFactory.CreateDbContext();

                int? accountId = await db.Accounts.Where(acc => acc.Email.ToLower().Equals(e.AccountEmail.ToLower())).Select(acc => acc.Id).FirstOrDefaultAsync();

            if (accountId==0)
            {
                Console.WriteLine("Error sending email");
                return;
            }
                 
            AccountEmail accountEmail =  new AccountEmail();

            accountEmail.AccountId = (int)accountId;

            Console.WriteLine($"[SENT EMAIL] {accountId} {e.AccountEmail}");

            accountEmail.EmailContent = $@"
            Email To: {e.AccountEmail}
            Action:   {e.Action}
            Entity:   {e.EntityType}
            Details:  {e.Description}
            Occurred: {e.OccurredAt}";

            await db.AccountEmails.AddAsync(accountEmail);
            await db.SaveChangesAsync();


            Console.WriteLine("[SENT EMAIL]");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[NotificationService] Failed: {ex.Message}");
            }
        }

        public void Dispose()
        {
            bus.OnEvent -= HandleEvent;
        }
    }
}
