using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimplifiedPayApi.Models;

namespace SimplifiedPayApi.Database.Mappings;

public class TransactionMap : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.ToTable("TRANSACTIONS");

        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id)
               .ValueGeneratedOnAdd()
               .UseIdentityColumn();

        builder.Property(t => t.Amount)
               .HasColumnType("MONEY")
               .IsRequired();

        builder.Property(t => t.Status)
               .HasColumnType("BIT")
               .IsRequired();

        builder.Property(t => t.Timestamp)
               .HasColumnType("DATETIME")
               .HasDefaultValue(DateTime.Now.ToUniversalTime());

        builder.HasOne(t => t.Payer)
               .WithMany()
               .HasConstraintName("FK_TRANSACTION_PAYER")
               .HasForeignKey(t => t.PayerId);
               
        builder.HasOne(t => t.Receiver)
               .WithMany()
               .HasConstraintName("FK_TRANSACTION_RECEIVER")
               .HasForeignKey(t => t.ReceiverId);
    }
}
