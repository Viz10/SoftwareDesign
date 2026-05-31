using AccountService.Infrastructure.DbRepository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Warehouse.Shared.Auth;
using Warehouse.Shared.Common;

namespace AccountService.Application.Queries
{
    internal record GetAccountEmailsQuery() : IRequest<Result<List<string>>>;

    internal class GetAccountEmailsQueryHandler(AccountServiceDbContext dbContext, CurrentUser currentUser) : IRequestHandler<GetAccountEmailsQuery,Result<List<string>>>
    {
        private readonly AccountServiceDbContext _dbContext = dbContext;
        private readonly CurrentUser _currentUser = currentUser;

        public async Task<Result<List<string>>> Handle(GetAccountEmailsQuery _ , CancellationToken ct)
        {
            var accountId = _currentUser.Id;

            var emails = await _dbContext.AccountEmails
                .Where(a => a.AccountId == accountId)
                .Select(a => a.EmailContent)
                .ToListAsync(ct);

            return (emails.Count == 0) ? Result<List<string>>.Fail("No emails found") : Result<List<string>>.Success(emails);
        }
    }
}
