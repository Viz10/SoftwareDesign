using AutoMapper;
using Warehouse.Data.Data.DTOs.ItemDTOs;
using Warehouse.Data.Entities;

namespace Warehouse.Mappings
{
    public class ItemMappingProfile : Profile
    {
        public ItemMappingProfile()
        {
            CreateMap<Item, ItemGetDTO>()
            .ForMember(dest => dest.Quantity,
             opt => opt.MapFrom(src => src.Stock.Quantity));
            /// the rest of matching members just copy

            CreateMap<ItemSendDTO, Item>();         
            CreateMap<ItemGetDTO, ItemSendDTO>();         
        }
    }
}
