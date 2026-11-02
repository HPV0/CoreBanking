using CoreBanking.Domain.Entities;
using CoreBanking.Domain.Entities.AccountEntities;
using CoreBanking.Domain.Entities.CurrencyEntities;

namespace TestHelper
{
    public static class TestEntityFactory
    {
        private static int _nextId;

        private static int NextId()
            => Interlocked.Increment(ref _nextId);

        public static Client CreateClient(
            string name = "Name",
            string surname = "Surname",
            string email = "email@mail.com",
            DateOnly? birthday = null,
            string passportNumber = "123123123123",
            ICollection<Account>? accounts = null)
        {
            return new Client
            {
                Name = name,
                Surname = surname,
                Email = email,
                Birthday = birthday,
                PassportNumber = passportNumber,
                Accounts = accounts?.ToList() ?? new List<Account>()
            };
        }

        public static Currency CreateCurrency(
            int? id = null,
            string currencyCode = "ABC",
            List<CurrencyRate>? currencyRates = null)
        {
            return new Currency
            {
                Id = id ?? NextId(),
                CurrencyCode = currencyCode,
                Rates = currencyRates?.ToList() ?? new List<CurrencyRate>()
            };
        }

        public static Account CreateAccount(
            int? id = null,
            Client? client = null,
            Currency? currency = null,
            decimal balance = 0)
        {
            client ??= CreateClient();
            currency ??= CreateCurrency();

            int accountId = id ?? NextId();

            return new Account
            {
                Id = accountId,
                CurrencyId = currency.Id,
                Currency = currency,
                Client = client,
                ClientId = client.Id,
                Balance = new()
                {
                    Amount = balance,
                    Id = accountId
                }
            };
        }

        public static CurrencyRate CreateCurrencyRate( decimal rate = 1, DateTimeOffset? validFrom = null, DateTimeOffset? validTo = null)
        {
            return new CurrencyRate
            {
                Rate = rate,
                ValidFrom = validFrom ?? DateTimeOffset.UtcNow,
                ValidTo = validTo
            };
        }
    }
}
