using CoreBanking.Domain.Entities.CurrencyEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreBanking.Infrastructure.Persistence.Configurations.CurrencyConfigurations
{
    public class CurrencyConfiguration : IEntityTypeConfiguration<Currency>
    {
        public void Configure(EntityTypeBuilder<Currency> builder)
        {
            BaseEntityConfiguration.Configure(builder);

            builder.Property(c => c.CurrencyCode)
                .HasMaxLength(3)
                .IsFixedLength()
                .IsRequired();
            
            builder.HasIndex(c => c.CurrencyCode)
                .IsUnique();

        }
    }
}
