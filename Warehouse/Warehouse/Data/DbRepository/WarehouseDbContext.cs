using Microsoft.EntityFrameworkCore;
using Warehouse.Data.Entities;

namespace Warehouse.Data.DbRepository
{
    public class WarehouseDbContext : DbContext
    {
        public DbSet<Item> Items { get; set; }

        public WarehouseDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var item = modelBuilder.Entity<Item>();

            item.HasIndex(x => x.Name).IsUnique().HasFilter("[IsDeleted] = 0");
            item.HasData(new Item {Id=1, Name = "TV", Description = "OLED SAMSUNG TV" }, new Item {Id=2, Name = "Gaming Monitor", Description = "240Hz Asus" });
            item.Property(p => p.LastModifiedTime).HasDefaultValueSql("SYSDATETIMEOFFSET()");  

        }
    }
}
