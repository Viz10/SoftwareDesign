using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Warehouse.Data;
using Warehouse.Data.Data.DTOs.StockUnitDTOs;
using Warehouse.Data.DbRepository;
using Warehouse.Data.Entities;

namespace Warehouse.Services
{
    public class StockUnitService : GenericService<StockUnit,StockUnitGetDTO, StockUnitSendDTO, StockUnitUpdateDTO>
    {
        public StockUnitService(WarehouseDbContext dbContext, IMapper mapper) : base(dbContext, mapper) { }

        public async override Task<Result<StockUnitGetDTO>> add(StockUnitSendDTO added)
        {

            using var tranzaction = await dbContext.Database.BeginTransactionAsync();

            try
            {
                var unitlocalUnits = new List<StockUnit>();

                for (int i = 0; i < added.Quantity; i++)
                {
                    unitlocalUnits.Add(new StockUnit
                    {
                        ItemId = added.ItemId,
                        SerialNumber = Guid.NewGuid().ToString("N").ToUpper()[..12],
                        Status = UnitStatus.Available,
                        CurrentPrice = added.CurrentPrice,
                        Note=added.Note
                    });
                }

                //// ADD STOCK UNITS
                await dbContext.StockUnits.AddRangeAsync(unitlocalUnits);

                //// UPDATE STOCK QUANTITY
                int rowsAffected = await dbContext.Stocks.Where(s => s.ItemId == added.ItemId).ExecuteUpdateAsync(
                    setter => setter
                    .SetProperty(p => p.Quantity, p => p.Quantity + added.Quantity)
                    .SetProperty(p => p.LastModifiedTime, DateTimeOffset.UtcNow)
                );

                //// ADD NEW
                if (rowsAffected == 0)
                {
                    dbContext.Stocks.Add(new Stock { ItemId = added.ItemId, Quantity = added.Quantity });
                }

                await dbContext.SaveChangesAsync();
                await tranzaction.CommitAsync();

                return Result<StockUnitGetDTO>.Success(null);
            }
            catch (Exception ex)
            {
                await tranzaction.RollbackAsync();
                return Result<StockUnitGetDTO>.Fail(ex.Message);
            }
        }
        public async override Task<Result<bool>> delete(int id)
        {
            await using var transaction = await dbContext.Database.BeginTransactionAsync();
            try
            {
                var itemId = await dbContext.StockUnits
                    .Where(su => su.Id == id)
                    .Select(su => su.ItemId)
                    .FirstOrDefaultAsync();

                if (itemId <= 0) return Result<bool>.Fail("Stock unit not found.");

                var result = await base.delete(id);
                if (!result.IsSuccessful) return result;

                int rowsAffected = await dbContext.Stocks
                    .Where(s => s.ItemId == itemId)
                    .ExecuteUpdateAsync(setter => setter
                        .SetProperty(p => p.Quantity, p => p.Quantity - 1)
                        .SetProperty(p => p.LastModifiedTime, DateTimeOffset.UtcNow)
                    );

                if (rowsAffected == 0) return Result<bool>.Fail("Stock summary record does not exist!");

                await transaction.CommitAsync();
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return Result<bool>.Fail(ex.Message);
            }
        }


        public async Task<Result<string>> GetItemName(int id)
        {
            var name = await dbContext.Items
                .Where(i => i.Id == id)
                .Select(i => i.Name)
                .FirstOrDefaultAsync();

            return name != null
                ? Result<string>.Success(name)
                : Result<string>.Fail("Item name not found");
        }
        public async Task<Result<List<StockUnitGetDTO>>> searchStockItemByBarcode(string? barCode)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(barCode)) return Result<List<StockUnitGetDTO>>.Fail("Barcode cannot be empty");

                var result = await dbContext.StockUnits
                    .Where(su => su.SerialNumber.ToLower() == barCode.ToLower())
                    .ProjectTo<StockUnitGetDTO>(mapper.ConfigurationProvider)
                    .ToListAsync();

                return result.Any()
                    ? Result<List<StockUnitGetDTO>>.Success(result)
                    : Result<List<StockUnitGetDTO>>.Fail($"No item found with barcode: {barCode}");
            }
            catch (Exception ex)
            {
                return Result<List<StockUnitGetDTO>>.Fail(ex.Message);
            }
        }
        public async Task<Result<List<StockUnitGetDTO>>> searchStockItemByItemName(string name)
        {
            try
            {
                var query = dbContext.StockUnits.AsQueryable();

                if (!string.IsNullOrWhiteSpace(name) && name.Length >= 2)
                {
                    query = query.Where(su => su.Item.Name.ToLower().Contains(name.ToLower()));
                }

                var result = await query
                    .ProjectTo<StockUnitGetDTO>(mapper.ConfigurationProvider)
                    .ToListAsync();

                return Result<List<StockUnitGetDTO>>.Success(result);
            }
            catch (Exception ex)
            {
                return Result<List<StockUnitGetDTO>>.Fail(ex.Message);
            }
        }
        public async Task<Result<List<StockUnitGetDTO>>> searchStockItemByItemId(int itemId)
        {
            try
            {
                var result = await dbContext.StockUnits
                    .Where(su => su.ItemId == itemId)
                    .ProjectTo<StockUnitGetDTO>(mapper.ConfigurationProvider)
                    .ToListAsync();

                return result.Any()
                    ? Result<List<StockUnitGetDTO>>.Success(result)
                    : Result<List<StockUnitGetDTO>>.Fail("No units found for this Item ID");
            }
            catch (Exception ex)
            {
                return Result<List<StockUnitGetDTO>>.Fail(ex.Message);
            }
        }
    }
}
