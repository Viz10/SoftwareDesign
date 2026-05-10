using AutoMapper;
using InventoryService.Application.Commands;
using InventoryService.Infrastructure.Entities;
using Warehouse.Shared.DTOs.ItemDTO;

namespace InventoryService.Application.Mappings
{
    public class ItemMappingProfile : Profile
    {
        public ItemMappingProfile()
        {
            CreateMap<Item, ItemGetResponse>()
            .ForMember(dest => dest.Quantity,  opt => opt.MapFrom(src => src.Stock != null ? src.Stock.Quantity : 0));
            /// Ensures that if Stock is null, Quantity defaults to 0

            CreateMap<AddItemRequest, AddItemCommand>();
            CreateMap<UpdateItemRequest, UpdateItemCommand>(); /// api conversions

            CreateMap<AddItemCommand, Item>();  /// service mapping       
            CreateMap<UpdateItemCommand, Item>();   /// service mapping       
            CreateMap<ItemGetResponse, UpdateItemRequest>(); /// refresh edit form        
        }
    }

}
