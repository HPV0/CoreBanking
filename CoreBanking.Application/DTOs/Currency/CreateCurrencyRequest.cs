namespace CoreBanking.Application.DTOs.Currency
{
    public class CreateCurrencyRequest
    {
        public string CurrencyCode { get; set; }
        public decimal Rate { get; set; }
    }
}
