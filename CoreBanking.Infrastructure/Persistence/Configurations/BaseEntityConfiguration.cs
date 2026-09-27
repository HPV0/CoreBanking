using CoreBanking.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoreBanking.Infrastructure.Persistence.Configurations
{
    public class BaseEntityConfiguration
    {
        public static void Configure<TEntity>(
        EntityTypeBuilder<TEntity> builder)
        where TEntity : BaseEntity
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.InsertedAt)
                .IsRequired()
                .HasDefaultValueSql("now()")
                .ValueGeneratedOnAdd();

            builder.Property(x => x.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

        }

    }
}
