using AutoMapper;
using Warehouse.Data.DTOs.ItemDTOs;
using Warehouse.Data.DTOs.StockUnitDTOs;
using Warehouse.Data.Entities;

namespace Warehouse.Mappings
{
    public class StockUnitMappingProfile : Profile
    {
        public StockUnitMappingProfile()
        {
            CreateMap<StockUnit, StockUnitResponseDTO>().ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Item.Name));

            CreateMap<StockUnitUpdateDTO, StockUnit>()
           .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Enum.Parse<UnitStatus>(src.Status)));
        }
    }
}
