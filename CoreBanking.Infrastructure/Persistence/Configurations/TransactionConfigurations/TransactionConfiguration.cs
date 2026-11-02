using CoreBanking.Domain.Constants;
using CoreBanking.Domain.Entities.TransactionEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoreBanking.Infrastructure.Persistence.Configurations.TransactionConfigurations
{
    public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            BaseEntityConfiguration.Configure(builder);

            builder.Property(t => t.Amount)
                .HasPrecision(18, FinancialPrecision.AmountPrecision)
                .IsRequired();

            builder.Property(x => x.TransactionId)
                .HasDefaultValueSql("txid_current()")
                .ValueGeneratedOnAdd();

            builder.Property(t => t.ExchangeRate)
                .HasPrecision(18, FinancialPrecision.RatePrecision)
                .IsRequired();

            builder.HasOne(t => t.AccountFrom)
                .WithMany()
                .HasForeignKey(t => t.AccountFromId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(t => t.AccountTo)
                .WithMany()
                .HasForeignKey(t => t.AccountToId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
