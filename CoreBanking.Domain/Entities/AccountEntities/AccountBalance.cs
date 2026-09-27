using CoreBanking.Application.Exceptions;
using CoreBanking.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreBanking.Domain.Entities.AccountEntities
{
    public class AccountBalance
    {
        public Guid Id {  get; private set; }
        public decimal Amount { get; private set; }

        private AccountBalance() { }
        public AccountBalance(Guid id, decimal amount) {
            if (Amount < 0)
                throw new ArgumentException("Balance's Amount can't be negative.");
            
            Amount = amount;
            Id = id;
        }


        public void Deposit(Money money)
        {
            Amount += money.Amount;
        }

        public void Withdraw(Money money)
        {
            if (money.Amount > Amount)
                throw new UnprocessableException("Insufficient funds.");

            Amount -= money.Amount;
        }


    }
}
