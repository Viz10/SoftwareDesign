using Microsoft.EntityFrameworkCore;
using Warehouse.Data.Entities;

namespace Warehouse.Data.DbRepository
{
    public class WarehouseDbContext : DbContext
    {
        public DbSet<Item> Items { get; set; } 
        public DbSet<StockUnit> StockUnits { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Account> Accounts { get; set; }

        public WarehouseDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var item = modelBuilder.Entity<Item>();
            item.HasQueryFilter(u => !u.IsDeleted);
            item.HasIndex(x => x.Name).IsUnique().HasFilter("[IsDeleted] = 0");
            item.Property(p => p.LastModifiedTime).HasDefaultValueSql("SYSDATETIMEOFFSET()");
            item.Property(p => p.PricePerItem).HasColumnType("decimal(18,2)");
            

            var stockUnit = modelBuilder.Entity<StockUnit>();
            stockUnit.HasQueryFilter(u => !u.IsDeleted);
            stockUnit.HasIndex(x => x.SerialNumber).IsUnique().HasFilter("[IsDeleted] = 0");
            stockUnit.Property(p => p.LastModifiedTime).HasDefaultValueSql("SYSDATETIMEOFFSET()");
            stockUnit.Property(p => p.ActualPrice).HasColumnType("decimal(18,2)");


            var stock = modelBuilder.Entity<Stock>();
            stock.HasQueryFilter(s => !s.Item.IsDeleted);
            stock.ToTable(t => t.HasCheckConstraint("pozitive_quantity_constraint","[Quantity] >= 0"));


            var account = modelBuilder.Entity<Account>();
            account.HasQueryFilter(a => !a.IsDeleted); /// when querying , skip deleted rows
            account.HasIndex(a => a.Email).IsUnique().HasFilter("[IsDeleted] = 0");
            account.Property(p => p.ModifiedTime).HasDefaultValueSql("SYSDATETIMEOFFSET()");
            account.Property(p => p.CreatedAt).HasDefaultValueSql("SYSDATETIMEOFFSET()");

        }
    }
}
