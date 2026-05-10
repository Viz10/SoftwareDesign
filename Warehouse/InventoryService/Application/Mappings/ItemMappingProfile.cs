using AutoMapper;
using InventoryService.Infrastructure.Entities;

namespace InventoryService.Application.Mappings
{
    public class ItemMappingProfile : Profile
    {
        public ItemMappingProfile()
        {
            //CreateMap<Item, ItemGetDTO>()
           // .ForMember(dest => dest.Quantity,
               //  opt => opt.MapFrom(src => src.Stock != null ? src.Stock.Quantity : 0));
            /// Ensures that if Stock is null, Quantity defaults to 0

            //CreateMap<ItemSendDTO, Item>();  /// service mapping       
            //CreateMap<ItemUpdateDTO, Item>();   /// service mapping       
            //CreateMap<ItemGetDTO, ItemUpdateDTO>(); /// refresh edit form        
        }
    }

}
