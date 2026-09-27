using CoreBanking.Domain.Entities.CurrencyEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreBanking.Domain.Constants
{
    public static class BaseCurrency
    {
        public readonly static Guid Id = new Guid("C1DF0956-C2D9-4FD4-A97C-87F4362BE184");
        public readonly static string CurrencyCode = "AMD";
        public static Currency Create()
        {
            var currency = Currency.Create(Id, CurrencyCode);
            var currencyRate = CurrencyRate.Create(1, currency, DateTimeOffset.UtcNow, null);
            currency.AddCurrencyRate(currencyRate);
            return currency;
        }
    }
}
