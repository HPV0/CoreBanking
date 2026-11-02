namespace CoreBanking.Application.DTOs.Currency
{
    public class CurrencyWithActiveRateResponseModel
    {
        public int Id { get; set; }
        public string CurrencyCode { get; set; }
        public CurrencyRateResponseModel? ActiveRate { get; set; }

    }
}
