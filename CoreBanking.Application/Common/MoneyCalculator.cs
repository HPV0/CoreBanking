using CoreBanking.Application.Interfaces.Common;
using CoreBanking.Domain.Constants;

namespace CoreBanking.Application.Common
{
    public class MoneyCalculator : IMoneyCalculator
    {
        public void ValidateExchangeRate(decimal rate)
        {
            if (rate <= 0)
                throw new ArgumentException("Rate cannot be zero or negative");
        }

        public void ValidateCurrencyCode(string currencyCode)
        {
            if (string.IsNullOrEmpty(currencyCode))
                throw new ArgumentException("Money currencyCode cannot be null or empty");

            if (currencyCode.Length != 3)
                throw new ArgumentException("Money currencyCode Length must be equal to 3");
        }

        public void NormalizeRate(ref decimal rate)
        {
            rate = Math.Round(rate, FinancialPrecision.RatePrecision);
        }

        public void NormalizeMoneyAmount(ref decimal amount)
        {
            amount = Math.Round(amount, FinancialPrecision.AmountPrecision);
        }

        public decimal NormalizeMoneyAmount(decimal amount)
        {
            return Math.Round(amount, FinancialPrecision.AmountPrecision);
        }

        public decimal ExchangeRateCalc(decimal currentRate, decimal targetRate)
        {
            ValidateExchangeRate(targetRate);
            ValidateExchangeRate(currentRate);

            NormalizeRate(ref targetRate);
            NormalizeRate(ref currentRate);

            var res = (currentRate / targetRate);

            NormalizeRate(ref res);

            return res;
        }

        public decimal ExchangeCalc(decimal amount, decimal currentRate, decimal targetRate)
        {
            var finalRate = ExchangeRateCalc(currentRate, targetRate);

            var finalAmount = amount * finalRate;

            NormalizeRate(ref finalAmount);

            return finalAmount;
        }

        public decimal ExchangeAmountCalc(decimal rate, decimal amount)
        {
            NormalizeRate(ref rate);

            NormalizeMoneyAmount(ref amount);

            var res = NormalizeMoneyAmount(rate * amount);

            return res;
        }
    }
}
