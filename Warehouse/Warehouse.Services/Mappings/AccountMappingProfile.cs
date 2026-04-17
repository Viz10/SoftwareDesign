using AutoMapper;
using Warehouse.Data.DTOs.AccountDTOs;
using Warehouse.Data.Entities;

public class AccountMappingProfile : Profile
{
    public AccountMappingProfile()
    {
        CreateMap<Account, AccountViewDTO>();
        CreateMap<AccountCreateDTO, Account>();
    }
}