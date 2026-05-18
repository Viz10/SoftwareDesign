using AccountService.Infrastructure.DbRepository;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Warehouse.Shared.Common;
using Warehouse.Shared.DTOs.AccountDTO;

namespace AccountService.Application.Commands
{
    public record LoginCommand(
        string Email,
        string Password) : IRequest<Result<Message>>;

    internal class LoginCommandHandler : IRequestHandler<LoginCommand, Result<Message>>
    {
        private readonly AccountServiceDbContext dbContext;
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;

        public LoginCommandHandler(AccountServiceDbContext _dbContext, IMapper _mapper, IConfiguration _configuration)
        {
            dbContext = _dbContext;
            mapper = _mapper;
            configuration = _configuration;
        }

        public async Task<Result<Message>> Handle(LoginCommand request, CancellationToken ct)
        {
            try
            { 
                var account = await dbContext.Accounts
               .Where(a => a.Email == request.Email)
               .FirstOrDefaultAsync(ct);

                if (account is null || !BCrypt.Net.BCrypt.Verify(request.Password, account.PasswordHashed))
                    return Result<Message>.Fail("Account does not exist or invalid email");

                var user = mapper.Map<User>(account);

                var token = CreateToken(user);

                return Result<Message>.Success(Message.CreateMessage(token));
            }
            catch (Exception ex)
            {
                return Result<Message>.Fail(ex.Message);
            }
        }
        private string CreateToken(User user)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.Role,user.AccountType.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["CreateJWT:Token"]!));

            var credentials = new SigningCredentials(key,SecurityAlgorithms.HmacSha512);

            var tokenDescriptor = new JwtSecurityToken(
                issuer: configuration["CreateJWT:Issuer"],
                audience: configuration["CreateJWT:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials:credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }
    }
}
