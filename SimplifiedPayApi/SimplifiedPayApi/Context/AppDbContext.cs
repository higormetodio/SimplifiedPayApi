using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using SimplifiedPayApi.Models;

namespace SimplifiedPayApi.Context;

public sealed class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }

    public DbSet<User> Users { get; set; }
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Transaction> Transactions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().ToTable("USERS");

        modelBuilder.Entity<User>().HasKey(u => u.Id);
        modelBuilder.Entity<User>().Property(u => u.Id)
                                   .ValueGeneratedOnAdd()
                                   .UseIdentityColumn();

        modelBuilder.Entity<User>().Property(u => u.FullName)
                                   .HasColumnType("NVARCHAR")
                                   .HasMaxLength(100)
                                   .IsRequired();

        modelBuilder.Entity<User>().HasAlternateKey(u => u.IdentificationNumebr);
        modelBuilder.Entity<User>().Property(u => u.IdentificationNumebr)
                                   .HasColumnType("NVARCHAR")
                                   .HasMaxLength(20)
                                   .IsRequired();

        modelBuilder.Entity<User>().HasAlternateKey(u => u.Email);
        modelBuilder.Entity<User>().Property(u => u.Email)
                                   .HasColumnType("NVARCHAR")
                                   .HasMaxLength(50)
                                   .IsRequired();

        modelBuilder.Entity<User>().Property(u => u.Password)
                                   .HasColumnType("NVARCHAR")
                                   .IsRequired();

        modelBuilder.Entity<User>().Property(u => u.UserType)
                                   .HasColumnType("NVARCHAR")
                                   .HasMaxLength(100)
                                   .IsRequired();

        modelBuilder.Entity<User>().HasMany(u => u.Transactions)
                                   .WithOne(t => t.Payer)
                                   .HasForeignKey(t => t.PayerId);

        modelBuilder.Entity<User>().HasMany(u => u.Transactions)
                                   .WithOne(t => t.Receiver)
                                   .HasForeignKey(t => t.ReceiverId);

        modelBuilder.Entity<User>().HasMany(u => u.Account)
                                   .WithOne(a => a.User)
                                   .HasForeignKey(a => a.UserId);

        modelBuilder.Entity<User>().HasIndex(u => u.IdentificationNumebr)
                                   .HasDatabaseName("IX_User_IdentificationNumber")
                                   .IsUnique();

        modelBuilder.Entity<User>().HasIndex(u => u.Email)
                                   .HasDatabaseName("IX_User_Email")
                                   .IsUnique();


    }
}
