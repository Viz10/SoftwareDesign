using AutoMapper;
using InventoryService.Infrastructure.Entities;

namespace InventoryService.Application.Mappings
{
    public class StockUnitMappingProfile : Profile
    {
        public StockUnitMappingProfile()
        {
            //CreateMap<StockUnit, StockUnitGetDTO>()
           // .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Item.Name));

           // CreateMap<StockUnitUpdateDTO, StockUnit>();
           // CreateMap<StockUnitSendDTO, StockUnit>();
           // CreateMap<StockUnitGetDTO, StockUnitUpdateDTO>();
        }
    }

}
