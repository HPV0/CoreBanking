using CoreBanking.Application.Exceptions;
using CoreBanking.Domain.Common;
using CoreBanking.Domain.Entities.CurrencyEntities;
using CoreBanking.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.InteropServices;
using System.Text;

namespace CoreBanking.Domain.Entities.AccountEntities
{
    public class Account : BaseEntity
    {
        public Guid ClientId { get; private set; }
        public Client Client { get; private set; }
        public Currency Currency { get; private set; }
        public Guid CurrencyId { get; private set; }
        public AccountBalance Balance { get; private set; }

        private Account() { }

        private Account(Guid id, Client client, Currency currency): this(client, currency)
        {

            Id = id;
            Balance = new AccountBalance(Id, 0);
        }


        private Account(Client client, Currency currency):base() {

            if (currency == null) 
                throw new ArgumentNullException("Currency cannot be null");
            
            if (client == null)
                throw new ArgumentNullException("Client cannot be null.");

            Client = client;
            ClientId = client.Id;
            CurrencyId = currency.Id;
            Currency = currency;
            Balance = new AccountBalance(Id, 0);
        }

        public void Deposit(Money money)
        {
            if (money.CurrencyCode != Currency.CurrencyCode)
                throw new UnprocessableException("Cannot Deposit, when diffrent currencies.");

            Balance.Deposit(money);
        }

        public void Withdraw(Money money)
        {
            if (money.CurrencyCode != Currency.CurrencyCode)
                throw new UnprocessableException("Cannot Withdraw, when diffrent currencies.");

            Balance.Withdraw(money);
        }

        public static Account Create(Currency currency, Client client) {
            return new Account(client, currency);
        }

        public static Account Create(Guid id, Currency currency, Client client)
        {
            return new Account(id, client, currency);
        }


    }
}
