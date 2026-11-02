namespace CoreBanking.Application.Interfaces.Common
{
    public interface IMoneyCalculator
    {
        public void ValidateExchangeRate(decimal rate);
        public void ValidateCurrencyCode(string currencyCode);
        public void NormalizeRate(ref decimal rate);
        public void NormalizeMoneyAmount(ref decimal amount);
        public decimal NormalizeMoneyAmount(decimal amount);
        public decimal ExchangeRateCalc(decimal currentRate, decimal targetRate);
        public decimal ExchangeAmountCalc(decimal rate, decimal amount);
        public decimal ExchangeCalc(decimal amount, decimal currentRate, decimal targetRate);
    }
}
