using InventoryService.Infrastructure.DbRepository;
using InventoryService.Infrastructure.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Warehouse.Shared.Common;

namespace InventoryService.Application.Commands
{
    public record DeleteStockUnitCommand(int id) : IRequest<Result<bool>>;

    public class DeleteStockUnitCommandHandler : IRequestHandler<DeleteStockUnitCommand, Result<bool>>
    {
        private readonly InventoryServiceDbContext dbContext;

        public DeleteStockUnitCommandHandler(InventoryServiceDbContext context)
        {
            dbContext = context;
        }

        public async Task<Result<bool>> Handle(DeleteStockUnitCommand command, CancellationToken ct)
        {
            await using var transaction = await dbContext.Database.BeginTransactionAsync(ct);
            try
            {
                var item = await dbContext.StockUnits.FindAsync(command.id);

                if (item is null)
                {
                    return Result<bool>.Fail("Stock unit not found.");
                }

                var itemId = item.ItemId;

                item.IsDeleted = true;
                item.LastModifiedTime = DateTimeOffset.UtcNow;

                int rowsAffected = await dbContext.Stocks
                    .Where(s => s.ItemId == itemId)
                    .ExecuteUpdateAsync(setter => setter
                        .SetProperty(p => p.Quantity, p => p.Quantity - 1)
                        .SetProperty(p => p.LastModifiedTime, DateTimeOffset.UtcNow)
                    );

                if (rowsAffected == 0) return Result<bool>.Fail("Stock summary record does not exist!");
                
                await dbContext.SaveChangesAsync(ct);
                await transaction.CommitAsync(ct);
                
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(ct);
                return Result<bool>.Fail(ex.Message);
            }
        }
    }

}
