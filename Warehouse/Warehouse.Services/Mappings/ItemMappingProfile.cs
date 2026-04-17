using AutoMapper;
using Warehouse.Data.DTOs.ItemDTOs;
using Warehouse.Data.Entities;

namespace Warehouse.Mappings
{
    public class ItemMappingProfile : Profile
    {
        public ItemMappingProfile()
        {
            CreateMap<Item, ItemGetDTO>();       
            CreateMap<ItemSendDTO, Item>();         
        }
    }
}
