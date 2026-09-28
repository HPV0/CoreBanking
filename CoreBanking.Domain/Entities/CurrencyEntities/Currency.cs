using CoreBanking.Application.Exceptions;
using CoreBanking.Domain.Common;
using CoreBanking.Domain.Entities.AccountEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreBanking.Domain.Entities.CurrencyEntities
{
    public class Currency : BaseEntity
    {
        public string CurrencyCode { get; private set; }
        private readonly List<CurrencyRate> _rates = [];
        public IReadOnlyCollection<CurrencyRate> Rates => _rates.AsReadOnly();

        private Currency() { }
        private Currency(Guid id, string currencyCode) : this(currencyCode)
        {
            Id = id;
        }

        private Currency(string currencyCode): base()
        {
            if (string.IsNullOrEmpty(currencyCode))
            {
                throw new ArgumentException("Currency Code cannot be null or Empty.");
            }

            if (currencyCode.Length != 3)
                throw new ArgumentException("Currency Code must have length of 3.");

            CurrencyCode = currencyCode;
        }

        public static Currency Create(string currencyCode) {
            return new Currency(currencyCode);
        }

        public static Currency Create(Guid id, string currencyCode)
        {
            return new Currency(id, currencyCode);
        }

        public CurrencyRate FindCurrentRate()
        {
            var currentRate = _rates.FirstOrDefault(cr => cr.ValidTo == null) ?? throw new UnprocessableException("Current Rate do not exist.");
            return currentRate;
        }
        public void AddCurrencyRate(CurrencyRate currencyrate)
        {
            if (currencyrate == null)
                throw new ArgumentNullException("Cannot add null currencyrate to Client.");

            var _rate = _rates.FirstOrDefault(a => a.Id == currencyrate.Id);
            if (_rate != null)
                throw new Exception("CurrecyRate already exist.");

            if (currencyrate.ValidTo == null)
            {
                var validToIsNull = _rates.FirstOrDefault(a => a.ValidTo == null);
                if (validToIsNull != null)
                    throw new UnprocessableException("Currency rates cannot have 2 values with ValidTo null.");
            }
            _rates.Add(currencyrate!);
        }

        public async void UpdateCurrentRate(decimal Rate)
        {
            var newCurrencyRateTime = DateTimeOffset.UtcNow;

            var currencyRate = this.FindCurrentRate();
            currencyRate.EndValid(newCurrencyRateTime);

            var newCurrencyRate = CurrencyRate.Create(Rate, this, newCurrencyRateTime, null);
            this.AddCurrencyRate(newCurrencyRate);

        }

    }

}

