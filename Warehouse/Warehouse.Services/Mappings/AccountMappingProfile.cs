using AutoMapper;
using Warehouse.Data.Data.DTOs.AccountDTOs;
using Warehouse.Data.Entities;

public class AccountMappingProfile : Profile
{
    public AccountMappingProfile()
    {
        CreateMap<RegisterDTO, Account>()
            .ForMember(dest => dest.PasswordHashed, opt => opt.Ignore());
    }
}