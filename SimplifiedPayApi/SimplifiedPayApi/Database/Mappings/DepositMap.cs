using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimplifiedPayApi.Models;

namespace SimplifiedPayApi.Database.Mappings
{
    public class DepositMap : IEntityTypeConfiguration<Deposit>
    {
        public void Configure(EntityTypeBuilder<Deposit> builder)
        {
            builder.ToTable("DEPOSITS");
            
            builder.HasKey(d => d.Id);
            builder.Property(d => d.Id)
                   .ValueGeneratedOnAdd()
                   .UseIdentityColumn();

            builder.HasOne(d => d.Depositor)
                   .WithMany(u => u.Deposits)
                   .HasConstraintName("FK_DEPOSIT_WALLET_DEPOSITOR")
                   .HasForeignKey(d => d.DepositorId)
                   .OnDelete(DeleteBehavior.ClientNoAction);
        }
    }
}
