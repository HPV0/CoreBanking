using CoreBanking.Domain.Entities.TransactionEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreBanking.Infrastructure.Persistence.Configurations.TransactionConfigurations
{
    public class TransactionDetailsConfiguration : IEntityTypeConfiguration<TransactionDetail>
    {
        public void Configure(EntityTypeBuilder<TransactionDetail> builder)
        {
            BaseEntityConfiguration.Configure(builder);

            builder.Property(t => t.Amount)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(t => t.ExchangeRate)
                .HasPrecision(18, 4)
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
