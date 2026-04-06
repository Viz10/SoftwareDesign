using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Warehouse.Data.DbRepository;
using Warehouse.Data.DTOs.AccountDTOs;
using Warehouse.Data.Entities;
using BCrypt.Net;

namespace Warehouse.Services
{
    public class AccountService : GenericService<Account, AccountViewDTO, AccountCreateDTO, AccountUpdateDTO>
    {
        public AccountService(WarehouseDbContext dbContext, IMapper mapper) : base(dbContext, mapper) { }



        public async Task<(bool,string?,AccountViewDTO?)> login(LoginDTO item)
        {
            var account = await dbContext.Set<Account>()
                .Where(a => a.Email == item.Email && a.IsActive)
                .FirstOrDefaultAsync();

            if (account is null)
                return (false,"Account with this email does not exist!!",null);

            bool isValid = BCrypt.Net.BCrypt.Verify(item.Password, account.PasswordHashed);

            if (isValid) return (true, null,mapper.Map<AccountViewDTO>(account));
            return (false, "Password Mismatch",null);
        }
        public async Task<(bool, string?, AccountViewDTO?)> register(AccountCreateDTO item)
        {
 
            if (item.Password != item.ConfirmPassword)
                return (false, "Passwords do not match.", null);

           
            bool emailTaken = await dbContext.Set<Account>()
                .AnyAsync(a => a.Email == item.Email && !a.IsDeleted);

            if (emailTaken)
                return (false, "An account with this email already exists.", null);

            var account = mapper.Map<Account>(item);
            account.PasswordHashed = BCrypt.Net.BCrypt.HashPassword(item.Password);

            await dbContext.Set<Account>().AddAsync(account);
            await dbContext.SaveChangesAsync();

            return (true, null, mapper.Map<AccountViewDTO>(account));
        }
    }
}