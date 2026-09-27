using CoreBanking.Domain.Entities;
using CoreBanking.Domain.ValueObjects;
using CoreBanking.Domain.ValueObjects.Client;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CoreBanking.Domain.ValueObjects
{
    public record class Money
    {
        public string CurrencyCode { get; }
        public decimal Amount { get; }
        private Money(decimal amount, string currencyCode)
        {
            if (amount < 0)
                throw new ArgumentException("Money amount cannot be negative");
            
            Money.ValidateCurrencyCode(currencyCode);

            Amount = amount;
            CurrencyCode = currencyCode;
        }

        public static void ValidateExchangeRate(decimal rate)
        {
            if (rate <= 0) throw new ArgumentException("Rate cannot be zero or negative");
        }

        public static void ValidateCurrencyCode(string currencyCode)
        {
            if (string.IsNullOrEmpty(currencyCode))
                throw new ArgumentException("Money currencyCode cannot be null or empty");

            if (currencyCode.Length != 3)
                throw new ArgumentException("Money currencyCode Length must be equal to 3");
        }

        public Money Exchange(decimal currentRate, decimal targetRate, string targetCurrencyCode) {
            Money.ValidateExchangeRate(targetRate);
            Money.ValidateExchangeRate(currentRate);

            return Money.Create((Amount / currentRate) * targetRate, targetCurrencyCode);
        }
        
        public Money ExchangeMoney(decimal exchangeRate, string currencyCode)
        {
            return Money.Create(Amount * exchangeRate, currencyCode);
        }

        public static Money Create(decimal amount, string currencyCode)
        {
            return new Money(amount, currencyCode);
        }

    }
}