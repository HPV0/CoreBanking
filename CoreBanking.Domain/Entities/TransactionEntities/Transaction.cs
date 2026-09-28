using CoreBanking.Application.Exceptions;
using CoreBanking.Domain.Common;
using CoreBanking.Domain.Entities.AccountEntities;
using CoreBanking.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreBanking.Domain.Entities.TransactionEntities
{
    public class Transaction : BaseEntity
    {

        private List<TransactionDetail> _details = new();
        public IReadOnlyCollection<TransactionDetail>? Details => _details?.ToList();
        private Transaction() { }
        private Transaction(Account sender, Account recevier, Account baseBankingAccount,
            decimal senderExchangeRate, decimal recevierExchangeRate,
            decimal amount): base()
        {

            Money sendingMoney = Money.Create(amount, sender.Currency.CurrencyCode);

            if (sender.Id == recevier.Id)
                throw new UnprocessableException("Sender and Recevier accounts cannot be same.");

            
            if (sender.CurrencyId == recevier.CurrencyId)
            {
                AddTransactionDetail(sender, recevier, 1, sendingMoney,out _);
            }
            else
            {
                Money baseAccountRecevingMoney;
                AddTransactionDetail(sender, baseBankingAccount, senderExchangeRate, sendingMoney, out baseAccountRecevingMoney);

                decimal recevierExchangeRateToBaseRate = 1 / recevierExchangeRate;
                AddTransactionDetail(baseBankingAccount, recevier, recevierExchangeRateToBaseRate, baseAccountRecevingMoney, out _);
            }

        }


        public static Transaction Create(Account sender, Account recevier, Account baseBankingAccount, 
            decimal senderExchangeRate, decimal recevierExchangeRate,
            decimal amount)
        {
            return new Transaction(sender, recevier, baseBankingAccount,
            senderExchangeRate, recevierExchangeRate, amount);
        }


        private void AddTransactionDetail(Account AccountFrom, Account AccountTo,
            decimal ExchangeRate, Money money, out Money AccountToMoney)
        {
            AccountFrom.Withdraw(money);
            AccountToMoney = money.ExchangeMoney(ExchangeRate, AccountTo.Currency.CurrencyCode);
            AccountTo.Deposit(AccountToMoney);

            TransactionDetail detail = TransactionDetail.Create(Id, AccountFrom, AccountTo, ExchangeRate, money);
            _details.Add(detail);
        }


    }
}
