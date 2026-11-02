using CoreBanking.Domain.Entities;
using CoreBanking.Domain.Entities.AccountEntities;
using CoreBanking.Domain.Entities.CurrencyEntities;

namespace CoreBanking.Domain.Constants
{
    public static class BankClient
    {
        public readonly static int Id = 1;
        public readonly static string Name = "Bank";
        public readonly static string Surname = "Bank";
        public readonly static string Email = "Bank@email.com";
        public readonly static string PassportNumber = "000000000000";
        public static Client Create(Currency currency)
        {
            var account = new Account { Id = BankAccount.Id, Currency = currency };

            var client = new Client
            {
                Id = Id,
                Name = Name,
                Surname = Surname,
                Email = Email,
                Birthday = null,
                PassportNumber = PassportNumber,
                Accounts = new List<Account> { account }
            };

            return client;
        }


    }
}
