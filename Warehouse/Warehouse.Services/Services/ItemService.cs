using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Warehouse.Data.Data.DTOs.ItemDTOs;
using Warehouse.Data.DbRepository;
using Warehouse.Data.Entities;
using Warehouse.Services.Services.Events;

namespace Warehouse.Services
{
    public class ItemService : GenericService<Item, ItemGetDTO, ItemSendDTO>
    {
        private readonly WarehouseEventBus _eventBus;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ItemService(WarehouseDbContext dbContext, IMapper mapper, WarehouseEventBus eventBus,IHttpContextAccessor httpContextAccessor) : base(dbContext, mapper)
        {
            _eventBus = eventBus;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<(List<ItemGetDTO>? Value, string? Error)> searchItemTypeByPartialName(string? name,string? sortBy)
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
                    if (sortBy.Equals("descending"))
                    {
                        query = query.OrderByDescending(el => el.Name);
                    }
                    else
                    {
                        query = query.OrderBy(el => el.Name);
                    }
                }

                var result = await query
                    .ProjectTo<ItemGetDTO>(mapper.ConfigurationProvider)
                    .ToListAsync(); /// return all

                return (result, null);
            }
            catch (Exception ex)
            {
                return (null, ex.Message);
            }
        }

        public override async Task<string?> add(ItemSendDTO item)
        {

            var (IsDuplicate, Error) = await isDuplicate(item.Name,null);

            if (Error is not null)
            {
                return Error; /// Exception
            }

            if (IsDuplicate)
            {
                return "Duplicate Item";
            }

            var res =  await base.add(item); /// clear

            if(res is not null)
            {
                return res;
            }

            _eventBus.Publish(new WarehouseEvent
            {
                AccountEmail = GetCurrentAccountName()??"",
                EntityType = "Item",
                Action = "Added",
                Description = $"Added :{item.Name}\n{item.ReferencePricePerItem}\n{item.Description}",
            });

            return res;
        }
        public override async Task<(ItemGetDTO? Value, string? Error)> edit(int id, ItemSendDTO updated)
        {
            var (IsDuplicate, Error) = await isDuplicate(updated.Name, id);

            if (Error is not null)
            {
                return (null,Error); /// Exception
            }

            if (IsDuplicate)
            {
                return (null,"Duplicate Item");
            }

            var res =  await base.edit(id, updated); /// clear

            if(res.Error is not null)
            {
                return (null,res.Error);
            }

            _eventBus.Publish(new WarehouseEvent
            {
                AccountEmail = GetCurrentAccountName() ?? "",
                EntityType = "Item",
                Action = "Edited",
                Description = $"Edited to: {updated.Name}\n{updated.ReferencePricePerItem}\n{updated.Description}",
            });

            return res;
        }
        public override async Task<string?> delete(int id)
        {
            try
            {
                var item = await dbContext.Items
                    .Include(i => i.Stock)
                    .FirstOrDefaultAsync(i => i.Id == id);

                if (item == null)
                {
                    return "Not present!";
                }

                /// Prevent deletion if StockUnits exist
                if (item.StockUnits.Any(su=>!su.IsDeleted))
                {
                    return "Cannot delete item: There are active Stock Units linked to it.";
                }

                var OldItem = item; /// for even bus

                /// No Stock units left , safe to delete stock data
                item.Stock.Quantity = 0;
                item.Stock.IsDeleted = true; 
                item.Stock.DeletedAtTime = DateTimeOffset.UtcNow;
                item.Stock.LastModifiedTime = DateTimeOffset.UtcNow;

                var res = await base.delete(id); 
                if (res is not null) return res;

                _eventBus.Publish(new WarehouseEvent
                {
                    AccountEmail = GetCurrentAccountName() ?? "",
                    EntityType = "Item",
                    Action = "Deleted",
                    Description = $"Deleted : {id}\n{OldItem.Name}\n{OldItem.ReferencePricePerItem}\n{OldItem.Description}",
                });

                return null;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        private async Task<(bool IsDuplicate, string? Error)> isDuplicate(string name,int? editItemId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    return (false, null);
                }
                else
                {
                    Item? result;

                    if (editItemId.HasValue) { /// edit case
                         result = await dbContext.Items
                                                .Where(item => item.Name.ToLower().Equals(name.ToLower()) && item.Id!=editItemId)
                                                .FirstOrDefaultAsync();
                    }
                    else /// add case
                    {
                        result = await dbContext.Items
                                                .Where(item => item.Name.ToLower().Equals(name.ToLower()))
                                                .FirstOrDefaultAsync();
                    }
                    if (result == null)
                        return (false, null);
                    
                    return (true, null);
                }
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }
        private string? GetCurrentAccountName()
        {
            return _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;
        } /// maybe should be moved in a static app singleton context...
    }
}
