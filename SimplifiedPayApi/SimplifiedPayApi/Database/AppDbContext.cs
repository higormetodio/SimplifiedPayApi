using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SimplifiedPayApi.Database.Mappings;
using SimplifiedPayApi.Models;

namespace SimplifiedPayApi.Context;

public sealed class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }

    public DbSet<Wallet> Users { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<Deposit> Deposits { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new WalletMap());
        modelBuilder.ApplyConfiguration(new TransactionMap());
        modelBuilder.ApplyConfiguration(new DepositMap());
    }
}
