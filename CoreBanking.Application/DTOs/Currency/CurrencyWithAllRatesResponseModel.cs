namespace CoreBanking.Application.DTOs.Currency
{
    public class CurrencyWithAllRatesResponseModel
    {
        public int Id { get; set; }
        public string CurrencyCode { get; set; }
        public List<CurrencyRateResponseModel> Rates { get; set; }
    }
}
