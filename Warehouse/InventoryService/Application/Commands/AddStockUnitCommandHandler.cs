
using InventoryService.Infrastructure.DbRepository;
using InventoryService.Infrastructure.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Warehouse.Shared.Common;

namespace InventoryService.Application.Commands
{
    internal record AddStockUnitCommand(int ItemId, decimal? CurrentPrice, string? Note, int Quantity) : IRequest<Result<string>>;


    internal class AddStockUnitCommandHandler(InventoryServiceDbContext _dbContext) : IRequestHandler<AddStockUnitCommand, Result<string>>
    {
        private readonly InventoryServiceDbContext dbContext = _dbContext;

        public async Task<Result<string>> Handle(AddStockUnitCommand command, CancellationToken ct)
        {
            using var tranzaction = await dbContext.Database.BeginTransactionAsync(ct);

            try
            {
                var unitlocalUnits = new List<StockUnit>();

                for (int i = 0; i < command.Quantity; i++)
                {
                    unitlocalUnits.Add(new StockUnit
                    {
                        ItemId = command.ItemId,
                        SerialNumber = Guid.NewGuid().ToString("N").ToUpper()[..12],
                        Status = UnitStatus.Available,
                        CurrentPrice = command.CurrentPrice,
                        Note = command.Note
                    });
                }

                //// ADD STOCK UNITS
                await dbContext.StockUnits.AddRangeAsync(unitlocalUnits,ct);

                //// UPDATE STOCK QUANTITY
                int rowsAffected = await dbContext.Stocks.Where(s => s.ItemId == command.ItemId).ExecuteUpdateAsync(setter => setter
                    .SetProperty(p => p.Quantity, p => p.Quantity + command.Quantity)
                    .SetProperty(p => p.LastModifiedTime, DateTimeOffset.UtcNow)
                    , cancellationToken: ct);

                //// ADD NEW
                if (rowsAffected == 0)
                {
                    dbContext.Stocks.Add(new Stock { ItemId = command.ItemId, Quantity = command.Quantity });
                }

                await dbContext.SaveChangesAsync(ct);
                await tranzaction.CommitAsync(ct);

                return Result<string>.Success("Added items to shelf");
            }
            catch (Exception ex)
            {
                await tranzaction.RollbackAsync(ct);
                return Result<string>.Fail(ex.Message);
            }
        }
    }
}
