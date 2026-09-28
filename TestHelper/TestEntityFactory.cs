using CoreBanking.Domain.Constants;
using CoreBanking.Domain.Entities;
using CoreBanking.Domain.Entities.AccountEntities;
using CoreBanking.Domain.Entities.CurrencyEntities;
using CoreBanking.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;

namespace TestHelper
{
    public static class TestEntityFactory
    {
        public static Client CreateClient(
            string name = "Name",
            string surname = "Surname",
            string email = "email@mail.com",
            DateOnly? birthday = null,
            string passportNumber = "123123123123",
            ICollection<Account>? accounts= null)
        {
            return Client.Create(
               name,
                surname,
                email,
                birthday,
                passportNumber,
                accounts
            );
        }

        public static Currency CreateCurrency(
            string currencyCode = "ABC")
        {
            return Currency.Create(currencyCode);
        }

        public static Money CreateMoney(
            decimal amount = 100,
            string currencyCode = "ABC")
        {
            return Money.Create(amount, currencyCode);
        }

        public static Account CreateAccount(
            Client? client = null,
            Currency? currency = null)
        {
            client ??= CreateClient();
            currency ??= CreateCurrency();

            return Account.Create(currency, client);
        }

        public static Account CreateAccount(
            Guid Id,
            Client? client = null,
            Currency? currency = null)
        {
            client ??= CreateClient();
            currency ??= CreateCurrency();

            return Account.Create(Id, currency, client);
        }

    }
}
