using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimplifiedPayApi.Models;
using System.Reflection.Emit;

namespace SimplifiedPayApi.Database.Mappings;

public class UserMap : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("USERS");

        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id)
               .ValueGeneratedOnAdd()
               .UseIdentityColumn();

        builder.Property(u => u.FullName)
               .HasColumnType("NVARCHAR")
               .HasMaxLength(100)
               .IsRequired();

        builder.HasAlternateKey(u => u.IdentificationNumebr);
        builder.Property(u => u.IdentificationNumebr)
               .HasColumnType("NVARCHAR")
               .HasMaxLength(20)
               .IsRequired();

        builder.HasAlternateKey(u => u.Email);
        builder.Property(u => u.Email)
                .HasColumnType("NVARCHAR")
                .HasMaxLength(50)
                .IsRequired();

        builder.Property(u => u.Password)
                .HasColumnType("NVARCHAR")
                .IsRequired();

        builder.Property(u => u.UserType)
                .HasColumnType("NVARCHAR")
                .HasMaxLength(100)
                .IsRequired();

        builder.HasIndex(u => u.IdentificationNumebr)
               .HasDatabaseName("IX_User_IdentificationNumber")
               .IsUnique();

        builder.HasIndex(u => u.Email)
               .HasDatabaseName("IX_User_Email")
               .IsUnique();
    }
}
