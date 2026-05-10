using AutoMapper;
using InventoryService.Application.Commands;
using InventoryService.Infrastructure.Entities;
using Warehouse.Shared.DTOs.ItemDTO;
using Warehouse.Shared.DTOs.StockUnitDTO;

namespace InventoryService.Application.Mappings
{
    public class StockUnitMappingProfile : Profile
    {
        public StockUnitMappingProfile()
        {
            CreateMap<StockUnit, StockUnitGetResponse>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Item.Name));

            CreateMap<AddStockUnitRequest, AddStockUnitCommand>();
            CreateMap<UpdateStockUnitRequest, UpdateStockUnitCommand>(); /// api conversions

            CreateMap<AddStockUnitCommand, StockUnit>();  /// service mapping       
            CreateMap<UpdateStockUnitCommand, StockUnit>();   /// service mapping       
            CreateMap<StockUnitGetResponse, UpdateStockUnitRequest>(); /// refresh edit form 
        }
    }

}
