using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Warehouse.Data.Data.DTOs.StockUnitDTOs;
using Warehouse.Data.DbRepository;
using Warehouse.Data.Entities;

namespace Warehouse.Services
{
    
    public class StockUnitService : GenericService<StockUnit,StockUnitGetDTO, StockUnitUpdateDTO>
    {
        public StockUnitService(WarehouseDbContext dbContext, IMapper mapper) : base(dbContext, mapper) { }

        public override Task<string?> add(StockUnitUpdateDTO added)
         => throw new NotSupportedException("Use add(StockUnitSendDTO) instead.");

        public async Task<string?> add(StockUnitSendDTO added)
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
                var stock = await dbContext.Stocks.FirstOrDefaultAsync(s => s.ItemId == added.ItemId);
                
                if (stock == null)
                    dbContext.Stocks.Add(new Stock { ItemId = added.ItemId, Quantity = added.Quantity });
                else
                    stock.Quantity += added.Quantity;

                await dbContext.SaveChangesAsync();
                await tranzaction.CommitAsync();

                return null;
            }
            catch (Exception ex)
            {
                await tranzaction.RollbackAsync();
                return ex.Message;
            }
        }
        public async override Task<string?> delete(int id)
        {
            await using var transaction = await dbContext.Database.BeginTransactionAsync();

            try
            {
                var (stockItem, error) = await base.findById(id);
                if (stockItem is null) return error;

                var stock = await dbContext.Stocks.FirstOrDefaultAsync(s => s.Item.Name == stockItem.Name);

                if (stock != null && stock.Quantity > 0)
                    stock.Quantity--;

                var res = await base.delete(id); /// saves both changes
                if (res is not null) return res;

                await transaction.CommitAsync();
                return null;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return ex.Message;
            }
        }
  

        public async Task<(string? Value, string? Msg)> GetItemName(int Id)
        {
            string? name = await dbContext.Items.Where(i => i.Id == Id).Select(su=>su.Name).FirstOrDefaultAsync();
            if (name == null) return (null, "Item name not found");
            return (name, null);
        }
        public async Task<(List<StockUnitGetDTO>? Value, string? Error)> searchStockItemByBarcode(string? barCode)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(barCode))
                {
                    return (null, "Not Found!");
                }

                var result = await dbContext.StockUnits
                    .Where(su => su.SerialNumber.ToLower().Equals(barCode.ToLower()))
                    .ProjectTo<StockUnitGetDTO>(mapper.ConfigurationProvider)
                    .ToListAsync();

                if (!result.Any())
                {
                    return (null, $"No item found with barcode: {barCode}");
                }

                return (result, null);
            }
            catch (Exception ex)
            {
                return (null, ex.Message);
            }
        }
        public async Task<(List<StockUnitGetDTO>? Value, string? Error)> searchStockItemByItemName(string name)
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

                return (result, null);
            }
            catch (Exception ex)
            {
                return (null, ex.Message);
            }
        }
        public async Task<(List<StockUnitGetDTO>? Value, string? Error)> searchStockItemByItemId(int itemId)
        {
            try
            {
                var result = await dbContext.StockUnits
                    .Where(su => su.ItemId == itemId)
                    .ProjectTo<StockUnitGetDTO>(mapper.ConfigurationProvider)
                    .ToListAsync();

                if (!result.Any())
                {
                    return (null, "Not found");
                }

                return (result, null);
            }
            catch (Exception ex)
            {
                return (null, ex.Message);
            }
        }
    }
    
}
