using Azure.Core;
using InventoryService.Infrastructure.DbRepository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Warehouse.Shared.Common;
using Warehouse.Shared.DTOs.ItemDTO;

namespace InventoryService.Application.Commands
{
    public record DeleteItemCommand(int id) : IRequest<Result<bool>>;

    public class DeleteItemCommandHandler : IRequestHandler<DeleteItemCommand, Result<bool>>
    {
        private readonly InventoryServiceDbContext dbContext;

        public DeleteItemCommandHandler(InventoryServiceDbContext context)
        {
            dbContext = context;
        }

        public async Task<Result<bool>> Handle(DeleteItemCommand command, CancellationToken ct)
        {
            try
            {
                var item = await dbContext.Items
                    .Include(i => i.Stock)
                    .Include(i => i.StockUnits)
                    .FirstOrDefaultAsync(i => i.Id == command.id,ct);

                if (item == null)
                {
                    return Result<bool>.Fail("Not present!");
                }

                /// Prevent deletion if StockUnits exist
                if (item.StockUnits.Any(su => !su.IsDeleted))
                {
                    return Result<bool>.Fail("Cannot delete item: There are active Stock Units linked to it.");
                }

                //var oldItemName = item.Name;
                //var oldPrice = item.ReferencePricePerItem;
                //var oldDesc = item.Description;

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

                /*
                _eventBus.Publish(new WarehouseEvent
                {
                    AccountEmail = GetCurrentAccountName() ?? "",
                    EntityType = "Item",
                    Action = "Deleted",
                    Description = $"Deleted : {id}\n{oldItemName}\n{oldPrice}\n{oldDesc}",
                });
                */
                
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Fail(ex.Message);
            }
        }
    }
}
