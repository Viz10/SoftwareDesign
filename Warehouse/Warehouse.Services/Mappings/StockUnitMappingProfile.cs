using AutoMapper;
using Warehouse.Data.Data.DTOs.StockUnitDTOs;
using Warehouse.Data.Entities;

namespace Warehouse.Mappings
{
    public class StockUnitMappingProfile : Profile
    {
        public StockUnitMappingProfile()
        {
            CreateMap<StockUnit, StockUnitGetDTO>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Item.Name));
            
            CreateMap<StockUnitUpdateDTO, StockUnit>();
            CreateMap<StockUnitSendDTO, StockUnit>();
            CreateMap<StockUnitGetDTO, StockUnitUpdateDTO>();
        }
    }
}
