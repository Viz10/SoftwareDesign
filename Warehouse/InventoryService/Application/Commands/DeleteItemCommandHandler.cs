using InventoryService.Infrastructure.DbRepository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Warehouse.Shared.Auth;
using Warehouse.Shared.Common;

namespace InventoryService.Application.Commands
{
    internal record DeleteItemCommand(int Id) : IRequest<Result>;


    internal class DeleteItemCommandHandler(
        InventoryServiceDbContext _dbContext,
        IHttpClientFactory httpFactory,
        CurrentUser user) : IRequestHandler<DeleteItemCommand, Result>
    {
        private readonly InventoryServiceDbContext dbContext = _dbContext;
        private readonly IHttpClientFactory _httpFactory = httpFactory;
        private readonly CurrentUser _user = user;

        public async Task<Result> Handle(DeleteItemCommand command, CancellationToken ct)
        {
            try
            {
                var item = await dbContext.Items
                    .Include(i => i.Stock)
                    .Include(i => i.StockUnits)
                    .FirstOrDefaultAsync(i => i.Id == command.Id,ct);

                if (item == null)
                {
                    return Result.Fail("Not present!");
                }

                /// Prevent deletion if StockUnits exist
                if (item.StockUnits.Any(su => !su.IsDeleted))
                {
                    return Result.Fail("Cannot delete item: There are active Stock Units linked to it.");
                }

                var oldItemName = item.Name;

                /// No Stock units left, safe to discard stock data
                if (item.Stock != null)
                {
                    item.Stock.Quantity = 0;
                    item.Stock.IsDeleted = true;
                    item.Stock.LastModifiedTime = DateTimeOffset.UtcNow;
                }

                item.IsDeleted = true;
                item.LastModifiedTime = DateTimeOffset.UtcNow;

                await dbContext.SaveChangesAsync(ct);

                var client = _httpFactory.CreateClient("NotificationService");
                await client.PostAsJsonAsync("api/notify", new WarehouseEvent
                {
                    EntityType = "Item",
                    AccountEmail = _user.Email,
                    Action = "Deleted",
                    Description = $"Item {oldItemName} was deleted",
                    OccurredAt = DateTimeOffset.UtcNow
                }, ct);

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Fail(ex.Message);
            }
        }
    }
}
