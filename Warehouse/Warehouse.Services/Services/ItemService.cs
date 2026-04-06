using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Warehouse.Data.DbRepository;
using Warehouse.Data.DTOs.ItemDTOs;
using Warehouse.Data.Entities;

namespace Warehouse.Services
{
    public class ItemService : GenericService<Item, ItemResponseDTO, ItemCreateDTO, ItemUpdateDTO>
    {
        public ItemService(WarehouseDbContext dbContext, IMapper mapper) : base(dbContext, mapper) { }

        public async Task<List<ItemResponseDTO>?> searchItemTypeByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name) || name.Length <2 )
            {
                Debug.WriteLine(name);
                return null;
            }

            var partialResults = await dbContext.Set<Item>()
            .Where(item => item.Name.Contains(name))
            .ToListAsync();

            if (!partialResults.Any()) {
                return null;
            }

            return mapper.Map<List<ItemResponseDTO>>(partialResults);
        }
    }
}
