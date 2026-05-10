using AccountService.Infrastructure.DbRepository;
using AccountService.Infrastructure.Entities;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Warehouse.Shared.Common;
using Warehouse.Shared.Enums;

namespace AccountService.Application.Commands
{
    public record RegisterAccountCommand(
        string Password,
        string ConfirmPassword,
        string Email,
        AccountType AccountType) : IRequest<Result<string>>; /// what handler returns for this command

    internal class RegisterAccountCommandHandler : IRequestHandler<RegisterAccountCommand, Result<string>>
    {

        private readonly AccountServiceDbContext dbContext;
        private readonly IMapper mapper;

        public RegisterAccountCommandHandler(AccountServiceDbContext _dbContext, IMapper _mapper)
        {
            dbContext = _dbContext;
            mapper = _mapper;
        }

        public async Task<Result<string>> Handle(RegisterAccountCommand request, CancellationToken ct)
        {
            try
            {
                bool emailTaken = await dbContext.Accounts.AnyAsync(a => a.Email == request.Email);
                if (emailTaken) return Result<string>.Fail("Email taken!");

                var account = mapper.Map<Account>(request);
                account.PasswordHashed = BCrypt.Net.BCrypt.HashPassword(request.Password);

                await dbContext.Accounts.AddAsync(account);
                await dbContext.SaveChangesAsync(ct);
                return Result<string>.Success("Registration done!");
            }
            catch (Exception ex)
            {
                return Result<string>.Fail(ex.Message);
            }
        }
    }
}
