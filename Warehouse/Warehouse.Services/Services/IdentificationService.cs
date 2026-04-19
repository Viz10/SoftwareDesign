using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Warehouse.Data.Data.DTOs.AccountDTOs;
using Warehouse.Data.DbRepository;
using Warehouse.Data.Entities;

namespace Warehouse.Services
{
    public class IdentificationService
    {
        protected readonly WarehouseDbContext dbContext;
        protected readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContextAccessor;

        public IdentificationService(WarehouseDbContext dbContext, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<string?> login(LoginDTO item)
        {
            try
            {
                var account = await dbContext.Accounts
               .Where(a => a.Email == item.Email)
               .FirstOrDefaultAsync();

                if (account is null || !BCrypt.Net.BCrypt.Verify(item.Password, account.PasswordHashed))
                    return "Invalid email or password";

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, account.Email),
                    new Claim(ClaimTypes.Role, account.AccountType.ToString()),
                    new Claim(ClaimTypes.NameIdentifier, account.Id.ToString())
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await httpContextAccessor.HttpContext!.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme, principal);

                return null;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }
        public async Task<string?> register(RegisterDTO item)
        {
            try
            {
                if (item.Password != item.ConfirmPassword)
                    return "Passwords do not match.";

                bool emailTaken = await dbContext.Accounts.AnyAsync(a => a.Email == item.Email);
                if (emailTaken) return "An account with this email already exists.";

                var account = mapper.Map<Account>(item);
                account.PasswordHashed = BCrypt.Net.BCrypt.HashPassword(item.Password);

                await dbContext.Accounts.AddAsync(account);
                await dbContext.SaveChangesAsync();
                return null;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public async Task<string?> logout()
        {
            await httpContextAccessor.HttpContext!.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);
            return null;
        }
    }
}