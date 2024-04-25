﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimplifiedPayApi.Models;

namespace SimplifiedPayApi.Database.Mappings;

public class DepositMap : IEntityTypeConfiguration<Deposit>
{
    public void Configure(EntityTypeBuilder<Deposit> builder)
    {
        builder.ToTable("ACCOUNTS");

        builder.HasKey(d => d.Id);
        builder.Property(d => d.Id)
               .ValueGeneratedOnAdd()
               .UseIdentityColumn();

        builder.Property(d => d.Amount)
               .HasColumnType("MONEY")
               .IsRequired();

        builder.Property(d => d.Timestamp)
               .HasColumnType("DATETIME")
               .HasDefaultValue(DateTime.Now.ToUniversalTime());

        builder.HasOne(d => d.User)
               .WithMany(u => u.Deposits)
               .HasConstraintName("FK_DEPOSIT_USER")
               .HasForeignKey(d => d.UserId);
               
        
    }
}