using CoreBanking.Application.Exceptions;
using CoreBanking.Domain.Common;
using CoreBanking.Domain.Entities.AccountEntities;
using CoreBanking.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreBanking.Domain.Entities.TransactionEntities
{
    public class TransactionDetail : BaseEntity
    {
        public Guid TransactionId { get; private set; }
        public Guid AccountFromId { get; private set; }
        public Account AccountFrom { get; private set; }
        public Guid AccountToId { get; private set; }
        public Account AccountTo { get; private set; }
        public decimal ExchangeRate { get; private set; }
        public decimal Amount { get; private set; }

        private TransactionDetail() { }

        public TransactionDetail(Guid transactionId, Account accountFrom, Account accountTo,
            decimal exchangeRate, Money money)
        {
            if (accountFrom == null) throw new ArgumentNullException("AccountFrom cannot be null.");
            if (accountTo == null) throw new ArgumentNullException("AccountTo cannot be null.");

            if(money.CurrencyCode != accountFrom.Currency.CurrencyCode) throw new UnprocessableException("Currency of sending money doesn't match reciver account currency.");

            Money.ValidateExchangeRate(exchangeRate);
            
            AccountFrom = accountFrom;
            AccountFromId = accountFrom.Id;
            AccountTo = accountTo;
            AccountToId = accountTo.Id;
            ExchangeRate = exchangeRate;
            Amount = money.Amount;
            TransactionId = transactionId;
        }
        
        public static TransactionDetail Create(Guid transactionId, Account AccountFrom, Account AccountTo,
            decimal ExchangeRate, Money money)
        {
            return new TransactionDetail(transactionId, AccountFrom, AccountTo,
            ExchangeRate, money);
        }
    }

}
