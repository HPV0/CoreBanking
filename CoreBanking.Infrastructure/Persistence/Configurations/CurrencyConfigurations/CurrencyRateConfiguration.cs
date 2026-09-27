using CoreBanking.Domain.Entities.CurrencyEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreBanking.Infrastructure.Persistence.Configurations.CurrencyConfigurations
{
    public class CurrencyRateConfiguration : IEntityTypeConfiguration<CurrencyRate>
    {
        public void Configure(EntityTypeBuilder<CurrencyRate> builder)
        {
            BaseEntityConfiguration.Configure(builder);
            
            builder.Property(cr => cr.Rate)
                .HasPrecision(18, 4)
                .IsRequired();

            builder.Property(cr => cr.ValidFrom)
                .IsRequired();

            builder
            .HasOne(cr => cr.Currency)
            .WithMany(c => c.Rates)
            .HasForeignKey(cr => cr.CurrencyId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();


        }
    }
}
