using CoreBanking.Domain.Constants;
using CoreBanking.Domain.Entities;
using CoreBanking.Domain.Entities.CurrencyEntities;
using CoreBanking.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net.Mime;
using System.Text;

namespace CoreBanking.Infrastructure.Persistence.Seed
{
    public static class DbInitializer
    {
        public static async Task SeedBankClientAndAccountAsync(ApplicationDbContext context)
        {

            if (await context.Clients.FindAsync(BankClient.Id) != null)
            {
                return; // Already seeded
            }

            var currency = await context.Currencies.FindAsync(BaseCurrency.Id) ?? throw new Exception("Base currency doesn't exist.");

            var bankClient = BankClient.Create(currency);
            context.Clients.Add(bankClient);

            await context.SaveChangesAsync();
        }

        public static async Task SeedBaseCurrencyAsync(ApplicationDbContext context)
        {
            if (await context.Currencies.FindAsync(BaseCurrency.Id) != null)
            {
                return; // Already seeded
            }
            var baseCurrency = BaseCurrency.Create();
            context.Currencies.Add(baseCurrency);

            await context.SaveChangesAsync();
        }


        public static async Task SeedAsync(ApplicationDbContext context)
        {

            await SeedBaseCurrencyAsync(context);
            await SeedBankClientAndAccountAsync(context);
        }
    }


}
