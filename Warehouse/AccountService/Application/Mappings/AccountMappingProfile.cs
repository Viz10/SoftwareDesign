using AccountService.Application.Commands;
using AccountService.Infrastructure.Entities;
using AutoMapper;
using Warehouse.Shared.DTOs.AccountDTO;

namespace AccountService.Application.Mappings
{
    public class AccountMappingProfile : Profile
    {
        public AccountMappingProfile()
        {
            CreateMap<SaveAccountEmailRequest, SaveAccountEmailCommand>();
            CreateMap<RegisterRequest, RegisterAccountCommand>();
            CreateMap<LoginRequest, LoginCommand>();

            CreateMap<Account, User>();
 
            CreateMap<RegisterAccountCommand, Account>().ForMember(dest => dest.PasswordHashed, opt => opt.Ignore());
        }
    }
}
