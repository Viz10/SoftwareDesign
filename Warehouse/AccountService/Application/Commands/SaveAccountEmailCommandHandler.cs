using AccountService.Infrastructure.DbRepository;
using AccountService.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Warehouse.Shared.Common;
using MediatR;

namespace AccountService.Application.Commands
{
    internal record SaveAccountEmailCommand(string AccountEmail,string Content) : IRequest<Result>;


    internal class SaveAccountEmailCommandHandler(AccountServiceDbContext dbContext) : IRequestHandler<SaveAccountEmailCommand, Result>
    {
        private readonly AccountServiceDbContext _dbContext = dbContext;
        public async Task<Result> Handle(SaveAccountEmailCommand cmd, CancellationToken ct)
        {
            var account = await _dbContext.Accounts
                .Where(a => a.Email.ToLower() == cmd.AccountEmail.ToLower())
                .FirstOrDefaultAsync(ct);

            if (account is null)
                return Result.Fail("Account not found");

            await _dbContext.AccountEmails.AddAsync(new AccountEmail
            {
                AccountId = account.Id,
                EmailContent = cmd.Content
            }, ct);

            await _dbContext.SaveChangesAsync(ct);
            return Result.Success();
        }
    }
}
