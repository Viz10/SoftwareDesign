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
        public async Task<(List<ItemGetDTO>? Value,string? Error)> searchItemTypeByName(string? name)
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
    }
}
