using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace InventoryService.Infrastructure.DbRepository
{
    public class InventoryServiceDbContextFactory :IDesignTimeDbContextFactory<InventoryServiceDbContext>
    {
        public InventoryServiceDbContext CreateDbContext(string[] args)
        {

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<InventoryServiceDbContext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

            return new InventoryServiceDbContext(optionsBuilder.Options);
        }
    }
}
