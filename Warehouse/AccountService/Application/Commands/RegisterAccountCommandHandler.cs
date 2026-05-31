using AccountService.Infrastructure.DbRepository;
using AccountService.Infrastructure.Entities;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Warehouse.Shared.Common;
using Warehouse.Shared.Enums;

namespace AccountService.Application.Commands
{
    internal record RegisterAccountCommand(
        string Password,
        string ConfirmPassword,
        string Email,
        AccountType AccountType) : IRequest<Result>; /// what handler returns for this command

    internal class RegisterAccountCommandHandler(AccountServiceDbContext _dbContext, IMapper _mapper) : IRequestHandler<RegisterAccountCommand, Result>
    {

        private readonly AccountServiceDbContext dbContext = _dbContext;
        private readonly IMapper mapper = _mapper;

        public async Task<Result> Handle(RegisterAccountCommand request, CancellationToken ct)
        {
            try
            {
                bool emailTaken = await dbContext.Accounts.AnyAsync(a => a.Email == request.Email, cancellationToken: ct);
                if (emailTaken) return Result.Fail("Email taken!");

                var account = mapper.Map<Account>(request);
                account.PasswordHashed = BCrypt.Net.BCrypt.HashPassword(request.Password);

                await dbContext.Accounts.AddAsync(account, ct);
                await dbContext.SaveChangesAsync(ct);
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Fail(ex.Message);
            }
        }
    }
}
