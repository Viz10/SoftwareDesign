using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Warehouse.Data;
using Warehouse.Data.Data.DTOs.ItemDTOs;
using Warehouse.Data.DbRepository;
using Warehouse.Data.Entities;
using Warehouse.Services.Services.Events;

namespace Warehouse.Services
{
    public class ItemService : GenericService<Item, ItemGetDTO, ItemSendDTO,ItemUpdateDTO>
    {
        private readonly WarehouseEventBus _eventBus;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ItemService(WarehouseDbContext dbContext,IMapper mapper,WarehouseEventBus eventBus,
            IHttpContextAccessor httpContextAccessor) : base(dbContext, mapper)
        {
            _eventBus = eventBus;
            _httpContextAccessor = httpContextAccessor;
        }

        
        public override async Task<Result<ItemGetDTO>> add(ItemSendDTO item)
        {
            try
            {
                var result_dup = await isDuplicate(item.Name, null);
                if (!result_dup.IsSuccessful) { return Result<ItemGetDTO>.Fail(result_dup.ErrorMsg); }
                if (result_dup.Value) { return Result<ItemGetDTO>.Fail("Duplicate Item"); }

                var result_restored = await Restore(item);
                if (!result_restored.IsSuccessful) { return Result<ItemGetDTO>.Fail(result_restored.ErrorMsg); }
                if(!result_restored.Value) {

                    var res = await base.add(item);
                    if (res is not null) { return res; }
                }

                _eventBus.Publish(new WarehouseEvent
                {
                    AccountEmail = GetCurrentAccountName() ?? "",
                    EntityType = "Item",
                    Action = "Added",
                    Description = $"Added :{item.Name}\n{item.ReferencePricePerItem}\n{item.Description}",
                });

                return Result<ItemGetDTO>.Success(null); ;
            }
            catch (Exception ex)
            {
                return Result<ItemGetDTO>.Fail(ex.Message);
            }
        }
        public override async Task<Result<ItemGetDTO>> edit(int id, ItemUpdateDTO updated)
        {
            try
            {
                var result_dup = await isDuplicate(updated.Name, id);
                if (!result_dup.IsSuccessful) { return Result<ItemGetDTO>.Fail(result_dup.ErrorMsg); }
                if (result_dup.Value) { return Result<ItemGetDTO>.Fail("Duplicate Item"); }

                var res = await base.edit(id, updated);
                if (!res.IsSuccessful) { return res; }

                _eventBus.Publish(new WarehouseEvent
                {
                    AccountEmail = GetCurrentAccountName() ?? "",
                    EntityType = "Item",
                    Action = "Edited",
                    Description = $"Edited to: {updated.Name}\n{updated.ReferencePricePerItem}\n{updated.Description}",
                });

                return res;
            }
            catch (Exception ex)
            {
                return Result<ItemGetDTO>.Fail(ex.Message);
            }
        }
        public override async Task<Result<bool>> delete(int id)
        {
            try
            {
                var item = await dbContext.Items
                    .Include(i => i.Stock)
                    .Include(i => i.StockUnits)
                    .FirstOrDefaultAsync(i => i.Id == id);

                if (item == null)
                {
                    return Result<bool>.Fail("Not present!");
                }

                /// Prevent deletion if StockUnits exist
                if (item.StockUnits.Any(su => !su.IsDeleted))
                {
                    return Result<bool>.Fail("Cannot delete item: There are active Stock Units linked to it.");
                }

                var oldItemName = item.Name;
                var oldPrice = item.ReferencePricePerItem;
                var oldDesc = item.Description;

                /// No Stock units left, safe to discard stock data
                if (item.Stock != null)
                {
                    item.Stock.Quantity = 0;
                    item.Stock.IsDeleted = true;
                    item.Stock.LastModifiedTime = DateTimeOffset.UtcNow;
                }

                var res = await base.delete(id);
                if (!res.IsSuccessful) return res;

                _eventBus.Publish(new WarehouseEvent
                {
                    AccountEmail = GetCurrentAccountName() ?? "",
                    EntityType = "Item",
                    Action = "Deleted",
                    Description = $"Deleted : {id}\n{oldItemName}\n{oldPrice}\n{oldDesc}",
                });

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Fail(ex.Message);
            }
        }


        /// For filtering
        public async Task<Result<List<ItemGetDTO>>> searchItemTypeByPartialName(string? name, string? sortBy)
        {
            try
            {
                var query = dbContext.Items.AsQueryable();

                if (!string.IsNullOrWhiteSpace(name) && name.Length >= 2)
                {
                    query = query.Where(item => item.Name.ToLower().Contains(name.ToLower()));
                }

                if (!string.IsNullOrWhiteSpace(sortBy))
                {
                    query = sortBy.Equals("descending", StringComparison.OrdinalIgnoreCase)
                        ? query.OrderByDescending(el => el.Name)
                        : query.OrderBy(el => el.Name);
                }

                var result = await query
                    .ProjectTo<ItemGetDTO>(mapper.ConfigurationProvider)
                    .ToListAsync();

                return Result<List<ItemGetDTO>>.Success(result);
            }
            catch (Exception ex)
            {
                return Result<List<ItemGetDTO>>.Fail(ex.Message);
            }
        }
        /// Check items for duplicate when adding/editing
        private async Task<Result<bool>> isDuplicate(string name, int? editItemId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    return Result<bool>.Success(false);
                }

                bool exists;
                if (editItemId.HasValue)
                {
                    exists = await dbContext.Items.AnyAsync(item => item.Name.ToLower() == name.ToLower() && item.Id != editItemId);
                }
                else
                {
                    exists = await dbContext.Items.AnyAsync(item => item.Name.ToLower() == name.ToLower());
                }

                return Result<bool>.Success(exists);
            }
            catch (Exception ex)
            {
                return Result<bool>.Fail(ex.Message);
            }
        }
        /// Instead of adding new, restore the old one with its stock with new data
        private async Task<Result<bool>> Restore(ItemSendDTO itemSend)
        {
            try
            {
                var deletedItem = await dbContext.Items.IgnoreQueryFilters()
                    .FirstOrDefaultAsync(i => i.Name.ToLower() == itemSend.Name.ToLower() && i.IsDeleted);

                if (deletedItem != null)
                {
                    deletedItem.IsDeleted = false;
                    deletedItem.LastModifiedTime = DateTimeOffset.UtcNow;

                    mapper.Map(itemSend, deletedItem);

                    var stock = await dbContext.Stocks.IgnoreQueryFilters()
                        .FirstOrDefaultAsync(s => s.ItemId == deletedItem.Id);

                    if (stock != null)
                        stock.IsDeleted = false;

                    await dbContext.SaveChangesAsync();
                    return Result<bool>.Success(true);
                }
                return Result<bool>.Success(false);
            }
            catch (Exception ex)
            {
                return Result<bool>.Fail(ex.Message);
            }
        }
        /// Restore one from deleted list and also its stock
        private async Task<Result<bool>> Restore(int itemId)
        {
            try
            {
                var deletedItem = await dbContext.Items.IgnoreQueryFilters()
                    .FirstOrDefaultAsync(i => i.Id == itemId && i.IsDeleted);

                if (deletedItem != null)
                {
                    deletedItem.IsDeleted = false;
                    deletedItem.LastModifiedTime = DateTimeOffset.UtcNow;

                    var stock = await dbContext.Stocks.IgnoreQueryFilters()
                        .FirstOrDefaultAsync(s => s.ItemId == deletedItem.Id);

                    if (stock != null)
                        stock.IsDeleted = false;

                    await dbContext.SaveChangesAsync();
                    return Result<bool>.Success(true);
                }
                return Result<bool>.Success(false);
            }
            catch (Exception ex)
            {
                return Result<bool>.Fail(ex.Message);
            }
        }


        private string? GetCurrentAccountName()
        {
            return _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;
        }
    }
}
