using AutoMapper;
using Warehouse.Data.DTOs.AccountDTOs;
using Warehouse.Data.Entities;

public class AccountMappingProfile : Profile
{
    public AccountMappingProfile()
    {
        CreateMap<Account, AccountViewDTO>();

        CreateMap<AccountCreateDTO, Account>()
            .ForMember(dest => dest.PasswordHashed, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.ModifiedTime, opt => opt.Ignore())
            .ForMember(dest => dest.DeletedAt, opt => opt.Ignore());

        CreateMap<AccountUpdateDTO, Account>();
    }
}