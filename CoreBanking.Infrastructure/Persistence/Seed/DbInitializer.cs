using CoreBanking.Domain.Constants;
using CoreBanking.Domain.Entities;
using CoreBanking.Domain.Entities.AccountEntities;
using CoreBanking.Domain.Entities.CurrencyEntities;
using CoreBanking.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace CoreBanking.Infrastructure.Persistence.Seed
{
    public static class DbInitializer
    {
        public static async Task SeedBankClientAsync(ApplicationDbContext context)
        {

            var bankClient = await context.Clients.SingleOrDefaultAsync(cl => cl.PassportNumber == BankClient.PassportNumber);

            if (bankClient != null)
            {
                return; // Already seeded
            }

            bankClient = new Client
            {
                Name = BankClient.Name,
                Birthday = null,
                Surname = BankClient.Surname,
                PassportNumber = BankClient.PassportNumber,
                Email = BankClient.Email
            };

            await context.Clients.AddAsync(bankClient);

            await context.SaveChangesAsync();
        }


        public static async Task SeedBankAccountAsync(ApplicationDbContext context)
        {

            var bankAccount = await context.Accounts.SingleOrDefaultAsync(acc => acc.IsBankAccount == true);

            var bankClient = await context.Clients.SingleOrDefaultAsync(cl => cl.PassportNumber == BankClient.PassportNumber)
                ?? throw new Exception("No bank client exist");

            var baseCurrency = await context.Currencies.SingleOrDefaultAsync(curr => curr.IsBaseCurrency == true)
                ?? throw new Exception("Base currency doesn't exist.");

            if (bankAccount != null)
            {
                if (bankAccount.ClientId != bankClient.Id) {
                    throw new Exception("Bank account client doesn't match with actual bank client");
                }

                if (bankAccount.CurrencyId != baseCurrency.Id)
                {
                    throw new Exception("Bank account currency doesn't match with actual base currency");
                }

                return; 
            }

            bankAccount = new Account { CurrencyId = baseCurrency.Id, ClientId = bankClient.Id, IsBankAccount = true, Balance = new AccountBalance { Amount = 0 } };

            await context.Accounts.AddAsync(bankAccount);

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

            await context.Currencies.AddAsync(baseCurrency);
            await context.CurrencyRates.AddAsync(baseCurrencyRate);

            await context.SaveChangesAsync();
        }

        public static async Task SeedAsync(ApplicationDbContext context)
        {
            await SeedBaseCurrencyAsync(context);

            await SeedBankClientAsync(context);

            await SeedBankAccountAsync(context);
        }
    }


}
