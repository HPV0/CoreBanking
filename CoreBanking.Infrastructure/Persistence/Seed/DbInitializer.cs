using CoreBanking.Domain.Constants;
using CoreBanking.Domain.Entities.CurrencyEntities;
using CoreBanking.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

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

            var currency = await context.Currencies.SingleOrDefaultAsync(curr => curr.IsBaseCurrency == true)
                ?? throw new Exception("Base currency doesn't exist.");

            var bankClient = BankClient.Create(currency);

            context.Clients.Add(bankClient);

            await context.SaveChangesAsync();
        }


        // I have moved from Hardcoded id model, to new "IsBaseCurrnecy" column, and in this logic I don't know if seeding is need ,
        // becouse the id of base can be changed in seeding, and this will complitly ruin whole app.
        public static async Task SeedBaseCurrencyAsync(ApplicationDbContext context)
        {
            if (await context.Currencies.SingleOrDefaultAsync(cur => cur.IsBaseCurrency == true) != null)
            {
                return; // Already seeded
            }

            var baseCurrency = new Currency { CurrencyCode = BaseCurrency.CurrencyCode, IsBaseCurrency = true };

            var baseCurrencyRate = new CurrencyRate { Rate = 1, Currency = baseCurrency, ValidFrom = DateTimeOffset.UtcNow, ValidTo = null };

            context.Currencies.Add(baseCurrency);
            context.CurrencyRates.Add(baseCurrencyRate);

            await context.SaveChangesAsync();
        }

        public static async Task SeedAsync(ApplicationDbContext context)
        {
            await SeedBaseCurrencyAsync(context);

            await SeedBankClientAndAccountAsync(context);
        }
    }


}
