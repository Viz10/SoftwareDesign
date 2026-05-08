using Microsoft.EntityFrameworkCore;
using System.Collections;
using InventoryService.Infrastructure.Entities;
using InventoryService.Infrastructure.DbRepository;

namespace InventoryService.Infrastructure.DbRepository
{
    public class InventoryServiceDbContext : DbContext
    {
        public DbSet<Item> Items { get; set; }
        public DbSet<StockUnit> StockUnits { get; set; }
        public DbSet<Stock> Stocks { get; set; }

        public InventoryServiceDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var item = modelBuilder.Entity<Item>();
            item.HasQueryFilter(u => !u.IsDeleted);
            item.HasIndex(x => x.Name).IsUnique().HasFilter("[IsDeleted] = 0");
            item.Property(p => p.CreatedAtTime).HasDefaultValueSql("SYSDATETIMEOFFSET()");
            item.Property(p => p.ReferencePricePerItem).HasColumnType("decimal(18,2)");


            var stockUnit = modelBuilder.Entity<StockUnit>();
            stockUnit.HasQueryFilter(u => !u.IsDeleted && !u.Item.IsDeleted);
            stockUnit.HasIndex(x => x.SerialNumber).IsUnique().HasFilter("[IsDeleted] = 0");
            stockUnit.Property(p => p.CreatedAtTime).HasDefaultValueSql("SYSDATETIMEOFFSET()");
            stockUnit.Property(p => p.CurrentPrice).HasColumnType("decimal(18,2)");


            var stock = modelBuilder.Entity<Stock>();
            stock.HasQueryFilter(s => !s.IsDeleted && !s.Item.IsDeleted);
            stock.ToTable(t => t.HasCheckConstraint("pozitive_quantity_constraint", "[Quantity] >= 0"));
            stock.Property(p => p.CreatedAtTime).HasDefaultValueSql("SYSDATETIMEOFFSET()");
        }
    }
}
