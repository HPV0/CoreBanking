using CoreBanking.Domain.Common;
using CoreBanking.Domain.Entities;
using CoreBanking.Domain.Entities.AccountEntities;
using CoreBanking.Domain.Entities.CurrencyEntities;
using CoreBanking.Domain.Entities.TransactionEntities;
using CoreBanking.Infrastructure.Persistence.Configurations;
using CoreBanking.Infrastructure.Persistence.Configurations.AccountConfigurations;
using CoreBanking.Infrastructure.Persistence.Configurations.CurrencyConfigurations;
using CoreBanking.Infrastructure.Persistence.Configurations.TransactionConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net;
using System.Reflection.Emit;
using System.Text;
using System.Xml;

namespace CoreBanking.Infrastructure.Data.Context
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Client> Clients => Set<Client>();
        public DbSet<Account> Accounts => Set<Account>();
        public DbSet<AccountBalance> AccountBalances => Set<AccountBalance>();
        public DbSet<Currency> Currencies => Set<Currency>();
        public DbSet<CurrencyRate> CurrencyRates => Set<CurrencyRate>();
        public DbSet<Transaction> Transactions=> Set<Transaction>();
        public DbSet<TransactionDetail> TransactionsDetails => Set<TransactionDetail>();

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }


        public override int SaveChanges()
            => SaveChanges(acceptAllChangesOnSuccess: true);

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            ApplyMyEntityOverrides();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
            => SaveChangesAsync(acceptAllChangesOnSuccess: true, cancellationToken);

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            ApplyMyEntityOverrides();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            ConfigureSoftDeleteFilter(builder);
            builder.ApplyConfiguration(new ClinetConfiguration());
            builder.ApplyConfiguration(new AccountConfiguration());
            builder.ApplyConfiguration(new AccountBalanceConfiguration());
            builder.ApplyConfiguration(new CurrencyConfiguration());
            builder.ApplyConfiguration(new CurrencyRateConfiguration());
            builder.ApplyConfiguration(new TransactionConfiguration());
            builder.ApplyConfiguration(new TransactionDetailsConfiguration());

            base.OnModelCreating(builder);
        }


        private static void ConfigureSoftDeleteFilter(ModelBuilder builder)
        {
            foreach (var softDeletableTypeBuilder in builder.Model.GetEntityTypes()
                .Where(x => typeof(ISoftDeletable).IsAssignableFrom(x.ClrType)))
            {
                var parameter = Expression.Parameter(softDeletableTypeBuilder.ClrType, "p");

                softDeletableTypeBuilder.SetQueryFilter(
                    Expression.Lambda(
                        Expression.Equal(
                            Expression.Property(parameter, nameof(ISoftDeletable.IsActive)),
                            Expression.Constant(true)),
                        parameter)
                );
            }
        }

        private void ApplyMyEntityOverrides()
        {
            //foreach (var entry in ChangeTracker.Entries<IAudited>())
            //{
            //    switch (entry.State)
            //    {
            //        case EntityState.Added:
            //            entry.Property(nameof(IAudited.CreatedAt)).CurrentValue = DateTime.UtcNow;
            //            break;
            //    }
            //}

            foreach (var entry in ChangeTracker.Entries<ISoftDeletable>())
            {
                switch (entry.State)
                {
                    case EntityState.Deleted:
                        entry.State = EntityState.Unchanged; 
                        entry.Property(nameof(ISoftDeletable.IsActive)).CurrentValue = false;
                        break;
                }
            }
        }
    }
}
