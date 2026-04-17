using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.Diagnostics;
using Warehouse.Data.DbRepository;
using Warehouse.Data.DTOs.ItemDTOs;
using Warehouse.Data.DTOs.StockUnitDTOs;
using Warehouse.Data.Entities;

namespace Warehouse.Services
{
    
    public class StockUnitService : GenericService<StockUnit,StockUnitGetDTO,StockUnitSendDTO>
    {
        public StockUnitService(WarehouseDbContext dbContext, IMapper mapper) : base(dbContext, mapper) { }

        
        public async override Task<(IEnumerable<StockUnitGetDTO>? Value, string? Error)> getAll()
        {
            try
            {
                var values = await dbContext.StockUnits
                .Select(u => new StockUnitGetDTO
                {
                    Id = u.Id,
                    Name = u.Item.Name,
                    ActualPrice = u.CurrentPrice,
                    SerialNumber = u.SerialNumber,
                    Note = u.Note,
                    Status = u.Status
                })
                .ToListAsync();

                return (values, null);

            }
            catch (Exception ex)
            {
                return (null,ex.Message);
            }
        }
        public async override Task<string?> add(StockUnitSendDTO added)
        {
            try
            {
                var units = new List<StockUnit>();

                for (int i = 0; i < added.Quantity; i++)
                {
                    string serial;
                    do
                    {
                        serial = Guid.NewGuid().ToString("N").ToUpper()[..12];
                    } while (await dbContext.StockUnits.AnyAsync(u => u.SerialNumber == serial));

                    units.Add(new StockUnit
                    {
                        ItemId = added.ItemId,
                        SerialNumber = serial,
                        Status = UnitStatus.Available,
                        CurrentPrice = added.ActualPrice
                    });
                }

                await dbContext.StockUnits.AddRangeAsync(units);

                var stock = await dbContext.Stocks.FirstOrDefaultAsync(s => s.ItemId == added.ItemId);

                if (stock == null)
                    dbContext.Stocks.Add(new Stock { ItemId = added.ItemId, Quantity = added.Quantity });
                else
                    stock.Quantity += added.Quantity;

                await dbContext.SaveChangesAsync();
                return null;
            }
            catch (DbException ex)
            {
                return ex.Message;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public async override Task<string?> delete(int id)
        {
            var unit = await dbContext.StockUnits.FindAsync(id);
            if (unit == null) return "Not found";

            unit.IsDeleted = true;
            unit.DeletedAtTime = DateTimeOffset.UtcNow;
            unit.LastModifiedTime = DateTimeOffset.UtcNow;

            var stock = await dbContext.Stocks.FirstOrDefaultAsync(s => s.ItemId == unit.ItemId);
            if (stock != null && stock.Quantity > 0)
                stock.Quantity--;

            await dbContext.SaveChangesAsync();
            return null;
        }

        public async Task<(StockUnitGetDTO? Value, string? Error)> searchStockItemByBarcode(string barCode)
        {
            if (string.IsNullOrWhiteSpace(barCode) || barCode.Length < 2)
            {
                return (null,"Invalid Barcode");
            }

            try
            {
                var Result = await dbContext.StockUnits
                .Where(element => element.SerialNumber.ToLower().Contains(barCode.ToLower()))
                .Select(u => new StockUnitGetDTO
                {
                    Id = u.Id,
                    Name = u.Item.Name,
                    ActualPrice = u.CurrentPrice,
                    SerialNumber = u.SerialNumber,
                    Note = u.Note,
                    Status = u.Status
                }).FirstOrDefaultAsync();

                    if (Result is null)
                    {
                        return (null, "Not Found");
                    }
                    return (Result, null);
            }
            catch (DbException ex)
            {
                return (null,ex.Message);
            }
            catch (Exception ex)
            {
                return (null,ex.Message);
            }
        }
        public async Task<(List<StockUnitGetDTO>? Value, string? Error)> searchStockItemByItemName(string name)
        {
            if (string.IsNullOrWhiteSpace(name) || name.Length < 2)
            {
                return (null, "Invalid Name");
            }

            try
            {
                var Results = await dbContext.StockUnits
                .Where(element => element.Item.Name.ToLower().Contains(name.ToLower()))
                .Select(u => new StockUnitGetDTO
                {
                    Id = u.Id,
                    Name = u.Item.Name,
                    ActualPrice = u.CurrentPrice,
                    SerialNumber = u.SerialNumber,
                    Note = u.Note,
                    Status = u.Status
                }).ToListAsync();

                if (Results.Count()==0)
                {
                    return (null, "Not Found");
                }
                return (Results, null);
            }
            catch (DbException ex)
            {
                return (null, ex.Message);
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
                var Results = await dbContext.StockUnits
                .Where(element => element.ItemId==itemId)
                .Select(u => new StockUnitGetDTO
                {
                    Id = u.Id,
                    Name = u.Item.Name,
                    ActualPrice = u.CurrentPrice,
                    SerialNumber = u.SerialNumber,
                    Note = u.Note,
                    Status = u.Status
                }).ToListAsync();

                if (Results.Count() == 0)
                {
                    return (null, "Not Found");
                }
                return (Results, null);
            }
            catch (DbException ex)
            {
                return (null, ex.Message);
            }
            catch (Exception ex)
            {
                return (null, ex.Message);
            }
        }
    }
    
}
