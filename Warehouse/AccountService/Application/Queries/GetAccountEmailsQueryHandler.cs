using AccountService.Infrastructure.DbRepository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using Microsoft.Identity.Client;
using System.Security.Claims;
using Warehouse.Shared.Auth;
using Warehouse.Shared.Common;
using Warehouse.Shared.DTOs.AccountDTO;

namespace AccountService.Application.Queries
{
    public record GetAccountEmailsQuery() : IRequest<Result<List<string>>>;

    public class GetAccountEmailsQueryHandler : IRequestHandler<GetAccountEmailsQuery,Result<List<string>>>
    {
        AccountServiceDbContext _dbContext;
        CurrentUser _currentUser;
        public GetAccountEmailsQueryHandler(AccountServiceDbContext dbContext,CurrentUser currentUser)
        {
           _dbContext = dbContext;
            _currentUser  = currentUser;
        }

        public async Task<Result<List<string>>> Handle(GetAccountEmailsQuery request, CancellationToken ct)
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
