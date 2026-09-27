using CoreBanking.Domain.Common;
using CoreBanking.Domain.Entities.AccountEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreBanking.Infrastructure.Persistence.Configurations.AccountConfigurations
{
    public class AccountConfiguration : IEntityTypeConfiguration<Account>
    {

        public void Configure(EntityTypeBuilder<Account> builder)
        {
            BaseEntityConfiguration.Configure(builder);


            builder.HasOne(a => a.Client)
                .WithMany(c => c.Accounts)
                .HasForeignKey(a => a.ClientId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Currency)
              .WithMany()
              .HasForeignKey(x => x.CurrencyId)
              .OnDelete(DeleteBehavior.NoAction);

            builder
            .HasOne(a => a.Balance)
            .WithOne()
            .HasForeignKey<AccountBalance>(b => b.Id)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();


        }
    }
}
