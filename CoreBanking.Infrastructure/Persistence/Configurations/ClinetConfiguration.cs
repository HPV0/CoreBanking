using CoreBanking.Domain.Entities;
using CoreBanking.Domain.ValueObjects.Client;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreBanking.Infrastructure.Persistence.Configurations
{
    public class ClinetConfiguration : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {

            BaseEntityConfiguration.Configure(builder);

            builder.Property(x => x.Name)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.Surname)
                .HasMaxLength (50)
                .IsRequired();

            builder.Property(x => x.PassportNumber).
                IsFixedLength()
                .HasMaxLength(Client.PassportNumberLength)
                .IsRequired();

            builder.Property(x => x.Email).HasConversion(
                email => email.Value,
                value => Email.Create(value)
                ).IsRequired();

            builder.HasIndex(x => x.PassportNumber)
                .IsUnique();

        }
    }
}
