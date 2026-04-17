using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Warehouse.Data.DbRepository;
using Warehouse.Data.DTOs.ItemDTOs;
using Warehouse.Data.Entities;

namespace Warehouse.Services
{
    public class ItemService : GenericService<Item, ItemGetDTO, ItemSendDTO>
    {
        public ItemService(WarehouseDbContext dbContext, IMapper mapper) : base(dbContext, mapper) { }
       
        public async Task<(List<ItemGetDTO>? Value,string? Error)> searchItemTypeByPartialName(string? name)
        {

            List<Item> result = new List<Item>();

            try
            {
                if (string.IsNullOrWhiteSpace(name) || name.Length < 2)
                {
                    result = await dbContext.Items.ToListAsync(); /// return all or nothing
                }
                else
                {
                    result = await dbContext.Items
                                            .Where(item => item.Name.ToLower().Contains(name.ToLower()))
                                            .ToListAsync(); /// partial result
                }
                return (mapper.Map<List<ItemGetDTO>>(result),null);
            }
            catch (Exception ex) {
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

            return await base.add(item); /// clear
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

            return await base.edit(id, updated); /// clear
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
    }
}
