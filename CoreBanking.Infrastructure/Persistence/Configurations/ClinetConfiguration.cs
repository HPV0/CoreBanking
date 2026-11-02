using CoreBanking.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoreBanking.Infrastructure.Persistence.Configurations
{
    public class ClinetConfiguration : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            BaseEntityConfiguration.Configure(builder);

            builder.Property(x => x.Name)
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(x => x.Surname)
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(x => x.PassportNumber).
                IsFixedLength()
                .HasMaxLength(12)
                .IsRequired();

            builder.Property(x => x.Email).
                HasMaxLength(40)
                .IsRequired();

            builder.HasIndex(x => x.PassportNumber)
                .IsUnique();

        }
    }
}
