using AutoMapper;
using Warehouse.Data.DbRepository;
using Warehouse.Data.DTOs.ItemDTOs;
using Warehouse.Data.Entities;

namespace Warehouse.Services
{
    public class ItemService : GenericService<Item, ItemResponseDTO, ItemCreateDTO, ItemUpdateDTO>
    {
        public ItemService(WarehouseDbContext dbContext, IMapper mapper) : base(dbContext, mapper) { }
    }
}
