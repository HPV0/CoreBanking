namespace CoreBanking.Application.DTOs.Currency
{
    public class UpdateCurrencyRateByCodeRequest
    {
        public string CurrencyCode { get; set; }
        public decimal Rate { get; set; }
    }
}
