using CoreBanking.Domain.Entities;
using CoreBanking.Domain.Entities.AccountEntities;
using CoreBanking.Domain.Entities.CurrencyEntities;

namespace CoreBanking.Domain.Constants
{
    public static class BankClient
    {
        public readonly static Guid Id = new Guid("AB9A95BF-DDBB-4339-B7C6-5C5E5E804076");
        public readonly static string Name = "Bank";
        public readonly static string Surname = "Bank";
        public readonly static string Email = "Bank@email.com";
        public readonly static string PassportNumber = "000000000000";
        public static Client Create(Currency currency)
        {
            var client = Client.Create(Id, Name, Surname, Email, null, PassportNumber, null);
            var account = Account.Create(BankAccount.Id, currency, client);
            
            client.AddAccount(account);
            
            return client;
        }


    }
}
