using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace AccountService.Infrastructure.DbRepository
{
    public class AccountServiceDbContextFactory : IDesignTimeDbContextFactory<AccountServiceDbContext>
    {
        public AccountServiceDbContext CreateDbContext(string[] args)
        {

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<AccountServiceDbContext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

            return new AccountServiceDbContext(optionsBuilder.Options);
        }
    }
}
