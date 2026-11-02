namespace CoreBanking.Application.DTOs.Currency
{
    public class CurrencyRateResponseModel
    {
        public int Id { get; set; }
        public DateTimeOffset ValidFrom { get; set; }
        public DateTimeOffset? ValidTo { get; set; }
        public decimal Rate { get; set; }
    }
}
