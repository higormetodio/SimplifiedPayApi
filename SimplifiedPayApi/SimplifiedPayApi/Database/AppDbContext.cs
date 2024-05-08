using Microsoft.EntityFrameworkCore;
using SimplifiedPayApi.Database.Mappings;
using SimplifiedPayApi.Models;

namespace SimplifiedPayApi.Context;

public sealed class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }

    public DbSet<Wallet> Users { get; set; }
    public DbSet<Transaction> Transactions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new WalletMap());
        modelBuilder.ApplyConfiguration(new TransactionMap());
    }
}
