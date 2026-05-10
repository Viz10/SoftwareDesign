using AccountService.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace AccountService.Infrastructure.DbRepository
{
    public class AccountServiceDbContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountEmail> AccountEmails { get; set; }

        public AccountServiceDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var account = modelBuilder.Entity<Account>();
            account.HasQueryFilter(a => !a.IsDeleted); /// when querying , skip deleted rows
            account.HasIndex(a => a.Email).IsUnique().HasFilter("[IsDeleted] = 0");
            account.Property(p => p.CreatedAtTime).HasDefaultValueSql("SYSDATETIMEOFFSET()");
        }
    }
}
