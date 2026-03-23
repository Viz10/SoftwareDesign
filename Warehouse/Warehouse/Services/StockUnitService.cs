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
    
    public class StockUnitService : GenericService<StockUnit,StockUnitResponseDTO,StockUnitCreateDTO,StockUnitUpdateDTO>
    {
        public StockUnitService(WarehouseDbContext dbContext, IMapper mapper) : base(dbContext, mapper) { }

        public async override Task<IEnumerable<StockUnitResponseDTO>> getAll()
        {
            return await dbContext.Set<StockUnit>()
                .Select(u => new StockUnitResponseDTO
                {
                    Id = u.Id,
                    Name = u.Item.Name,
                    ActualPrice = u.ActualPrice,
                    SerialNumber = u.SerialNumber,
                    Note = u.Note,
                    Status = u.Status
                })
                .ToListAsync();
        }
        public async Task<List<StockUnitResponseDTO>?> searchStockItemByBarcode(string barCode)
        {
            if (string.IsNullOrWhiteSpace(barCode) || barCode.Length < 2)
            {
                Debug.WriteLine(barCode);
                return null;
            }

            var partialResults = await dbContext.Set<StockUnit>().Include(el=>el.Item)
            .Where(element=>element.SerialNumber.Contains(barCode))
            .ToListAsync();

            if (!partialResults.Any())
            {
                return null;
            }

            return mapper.Map<List<StockUnitResponseDTO>>(partialResults);
        }
        public async Task<List<StockUnitResponseDTO>?> searchStockItemByItemName(string name)
        {
            if (string.IsNullOrWhiteSpace(name) || name.Length < 2)
            {
                Debug.WriteLine(name);
                return null;
            }

            var partialResults = await dbContext.Set<StockUnit>().Include(a=>a.Item).Where(item => item.Item.Name.Contains(name))
                .Select(item => new StockUnitResponseDTO
                                {
                                    Name = item.Item.Name,
                                    ActualPrice = item.ActualPrice ?? 0m,
                                    SerialNumber = item.SerialNumber,
                                    Note = item.Note,
                                    Status = item.Status,
                                    Id = item.Id
                                 }).ToListAsync();
            return partialResults;
        }
        public async Task<List<StockUnitResponseDTO>?> searchStockItemByItemId(int itemId)
        {
            var itemExists = await dbContext.Set<Item>()
                .AnyAsync(i => i.Id == itemId && !i.IsDeleted);

            if (!itemExists) return null;

            return await dbContext.Set<StockUnit>()
                .Include(u => u.Item)
                .Where(u => u.ItemId == itemId)
                .Select(u => new StockUnitResponseDTO
                {
                    Id = u.Id,
                    Name = u.Item.Name,
                    ActualPrice = u.ActualPrice,
                    SerialNumber = u.SerialNumber,
                    Note = u.Note,
                    Status = u.Status
                })
                .ToListAsync();
        }
        public async override Task<StockUnitCreateDTO?> add(StockUnitCreateDTO added)
        {
            try
            {
                var units = new List<StockUnit>();

                for (int i = 0; i < added.Quantity; i++)
                {
                    units.Add(new StockUnit
                    {
                        ItemId = added.ItemId,
                        SerialNumber = Guid.NewGuid().ToString("N").ToUpper()[..12],
                        Status = UnitStatus.Available,
                        ActualPrice=added.PricePerItem
                    });
                }

                await dbContext.Set<StockUnit>().AddRangeAsync(units);

                var stock = await dbContext.Set<Stock>().FirstOrDefaultAsync(s => s.ItemId == added.ItemId);
                
                if (stock == null)
                    dbContext.Set<Stock>().Add(new Stock { ItemId = added.ItemId, Quantity = added.Quantity });
                else
                    stock.Quantity += added.Quantity;

                await dbContext.SaveChangesAsync();
                return added;
            }
            catch (DbException)
            {
                return null;
            }
        }

        private async Task<string?> getNameFromItemId(int itemId) 
        {
            return await dbContext.Set<Item>().Where(el => el.Id == itemId).Select(item => item.Name).FirstOrDefaultAsync();
        }
    }
    
}
