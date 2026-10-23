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

            NormalizeMoneyAmount(ref amount);
            
            Amount = amount;
            CurrencyCode = currencyCode;
        }

        public static void ValidateExchangeRate(decimal rate)
        {
            if (rate <= 0) 
                throw new ArgumentException("Rate cannot be zero or negative");
        }

        public static void ValidateCurrencyCode(string currencyCode)
        {
            if (string.IsNullOrEmpty(currencyCode))
                throw new ArgumentException("Money currencyCode cannot be null or empty");

            if (currencyCode.Length != 3)
                throw new ArgumentException("Money currencyCode Length must be equal to 3");
        }

        public static void NormalizeRate(ref decimal rate) {
            rate = Math.Round(rate, 6);
        }

        public static void NormalizeMoneyAmount(ref decimal amount)
        {
            amount = Math.Round(amount, 4);
        }

        public static decimal NormalizeMoneyAmount(decimal amount)
        {
            return Math.Round(amount, 4);
        }

        public static decimal ExchangeCalc(decimal currentRate, decimal targetRate) {
            Money.ValidateExchangeRate(targetRate);
            Money.ValidateExchangeRate(currentRate);

            Money.NormalizeRate(ref targetRate);
            Money.NormalizeRate(ref currentRate);

            var res = (currentRate / targetRate);

            NormalizeRate(ref res);
            
            return res;
        }

        public Money Exchange(decimal currentRate, decimal targetRate, string targetCurrencyCode, out decimal finalExchangeRate)
        {
            finalExchangeRate = ExchangeCalc(currentRate, targetRate);

            return Money.Create(Amount * finalExchangeRate, targetCurrencyCode);
        }

        public static Money Create(decimal amount, string currencyCode)
        {
            return new Money(amount, currencyCode);
        }

    }
}