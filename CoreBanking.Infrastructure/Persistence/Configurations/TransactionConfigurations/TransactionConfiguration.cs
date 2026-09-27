using CoreBanking.Domain.Entities.TransactionEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreBanking.Infrastructure.Persistence.Configurations.TransactionConfigurations
{
    public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            BaseEntityConfiguration.Configure(builder);


            builder.HasMany(t => t.Details)
                .WithOne()
                .HasForeignKey(td => td.TransactionId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);


        }
    }
}
