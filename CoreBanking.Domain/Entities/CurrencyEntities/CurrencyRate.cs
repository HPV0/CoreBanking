using CoreBanking.Application.Exceptions;
using CoreBanking.Domain.Common;
using CoreBanking.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreBanking.Domain.Entities.CurrencyEntities
{
    public class CurrencyRate : BaseEntity
    {
        public Guid CurrencyId { get; private set; }
        public Currency Currency { get; private set; }
        public decimal Rate { get; private set; }
        public DateTimeOffset ValidFrom { get; private set; }
        public DateTimeOffset? ValidTo { get; private set; }
        private CurrencyRate() { }


        private CurrencyRate(decimal rate, Currency currency, DateTimeOffset validFrom, DateTimeOffset? validTo) : base()
        {
            if (validFrom > DateTimeOffset.Now)
                throw new ArgumentException("Currnecy validFrom must be in past.");

            if (validTo != null && validTo <= validFrom)
                throw new ArgumentException("validFrom must be later then validTo.");

            UpdateRate(rate);
            Currency = currency;
            CurrencyId = currency.Id;
            ValidFrom = validFrom; 
            ValidTo = validTo;
        }

        public static CurrencyRate Create(decimal rate, Currency currency, DateTimeOffset validFrom, DateTimeOffset? validTo)
        {
            return new CurrencyRate(rate, currency, validFrom, validTo);
        }

        public void EndValid(DateTimeOffset endValidTime)
        {
            if (ValidTo != null)
                throw new UnprocessableException("CurrencyRate is already not valid.");
            
            ValidTo = endValidTime;
        }

        public void UpdateRate(decimal rate)
        {
            Money.ValidateExchangeRate(rate);

            Rate = rate;
        }

    }
}
