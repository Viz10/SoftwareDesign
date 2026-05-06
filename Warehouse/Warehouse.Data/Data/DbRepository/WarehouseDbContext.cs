using Microsoft.EntityFrameworkCore;
using Warehouse.Data.Data.Entities;
using Warehouse.Data.Entities;

namespace Warehouse.Data.DbRepository
{

    ///Add-Migration Init -Project Warehouse.Data -StartupProject Warehouse.Web
    ///Update-database -Project Warehouse.Data -StartupProject Warehouse.Web

    public class WarehouseDbContext : DbContext
    {
        public DbSet<Item> Items { get; set; } 
        public DbSet<StockUnit> StockUnits { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderLine> OrderLines { get; set; }
        public DbSet<Receipt> Receipts { get; set; }
        public DbSet<ReceiptLine> ReceiptLines { get; set; }
        public DbSet<AccountEmail> AccountEmails { get; set; }
       


        public WarehouseDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

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
            stock.ToTable(t => t.HasCheckConstraint("pozitive_quantity_constraint","[Quantity] >= 0"));
            stock.Property(p => p.CreatedAtTime).HasDefaultValueSql("SYSDATETIMEOFFSET()");


            var account = modelBuilder.Entity<Account>();
            account.HasQueryFilter(a => !a.IsDeleted); /// when querying , skip deleted rows
            account.HasIndex(a => a.Email).IsUnique().HasFilter("[IsDeleted] = 0");
            account.Property(p => p.CreatedAtTime).HasDefaultValueSql("SYSDATETIMEOFFSET()");


            var order = modelBuilder.Entity<Order>();
            order.Property(p => p.CreatedAtTime).HasDefaultValueSql("SYSDATETIMEOFFSET()");
            order.HasIndex(o => new { o.CustomerId, o.Status });
            order.HasIndex(o => new { o.SellerId, o.Status });
            order.HasOne(o => o.Customer)
                 .WithMany()
                 .HasForeignKey(o => o.CustomerId)
                 .OnDelete(DeleteBehavior.Restrict);
            order.HasOne(o => o.Seller)
                 .WithMany()
                 .HasForeignKey(o => o.SellerId)
                 .OnDelete(DeleteBehavior.Restrict);


            var receipt = modelBuilder.Entity<Receipt>();
            receipt.Property(p => p.GeneratedAt).HasDefaultValueSql("SYSDATETIMEOFFSET()");
            receipt.Property(p => p.Subtotal).HasColumnType("decimal(18,2)");
            receipt.Property(p => p.Tax).HasColumnType("decimal(18,2)");
            receipt.Property(p => p.Total).HasColumnType("decimal(18,2)");
            receipt.ToTable(t => t.HasCheckConstraint("pozitive_quantity_Subtotal_NonNegative", "[Subtotal] >= 0"));
            receipt.ToTable(t => t.HasCheckConstraint("pozitive_quantity_Tax_NonNegative", "[Tax] >= 0"));
            receipt.ToTable(t => t.HasCheckConstraint("pozitive_quantity_Total_NonNegative", "[Total] >= 0"));
            receipt.HasIndex(p => p.GeneratedAt);
            receipt.HasIndex(p => p.BuyerEmail);


            var receiptLine = modelBuilder.Entity<ReceiptLine>();
            receiptLine.Property(p => p.UnitPrice).HasColumnType("decimal(18,2)");
            receiptLine.Property(p => p.LineTotal).HasColumnType("decimal(18,2)");
            receiptLine.ToTable(t => t.HasCheckConstraint("pozitive_quantity_Quantity_NonNegative", "[Quantity] >= 0"));
            receiptLine.ToTable(t => t.HasCheckConstraint("pozitive_quantity_UnitPrice_NonNegative", "[UnitPrice] >= 0"));
            receiptLine.ToTable(t => t.HasCheckConstraint("pozitive_quantity_LineTotal_NonNegative", "[LineTotal] >= 0"));  
        }
    }
}
