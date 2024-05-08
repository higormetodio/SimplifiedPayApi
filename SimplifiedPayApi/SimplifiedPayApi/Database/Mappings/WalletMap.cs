using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimplifiedPayApi.Models;
using System.Reflection.Emit;

namespace SimplifiedPayApi.Database.Mappings;

public class WalletMap : IEntityTypeConfiguration<Wallet>
{
    public void Configure(EntityTypeBuilder<Wallet> builder)
    {
        builder.ToTable("WALLETS");

        builder.HasKey(w => w.Id);
        builder.Property(w => w.Id)
               .ValueGeneratedOnAdd()
               .UseIdentityColumn();

        builder.Property(w => w.FullName)
               .HasColumnType("NVARCHAR")
               .HasMaxLength(100)
               .IsRequired();

        builder.HasAlternateKey(w => w.IdentificationNumber)
               .HasName("UQ_WALLETS_IDENTIFICATION_NUMBER");
        builder.Property(w => w.IdentificationNumber)
               .HasColumnType("NVARCHAR")
               .HasMaxLength(20)
               .IsRequired();

        builder.HasAlternateKey(w => w.Email)
               .HasName("UQ_WALLETS_EMAIL");
        builder.Property(w => w.Email)
                .HasColumnType("NVARCHAR")
                .HasMaxLength(50)
                .IsRequired();

        builder.Property(w => w.Password)
                .HasColumnType("NVARCHAR")
                .HasMaxLength(255)
                .IsRequired();

        builder.Property(w => w.Balance)
               .HasColumnType("MONEY")
               .IsRequired();

        builder.Property(w => w.UserType)
                .HasColumnType("NVARCHAR")
                .HasMaxLength(100)
                .IsRequired();

        builder.HasIndex(w => w.IdentificationNumber)
               .HasDatabaseName("IX_WALLETS_IDENTIFICATION_NUMBER")
               .IsUnique();

        builder.HasIndex(w => w.Email)
               .HasDatabaseName("IX_WALLETS_EMAIL")
               .IsUnique();
    }
}
