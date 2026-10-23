namespace CoreBanking.Application.DTOs.Currency
{
    public class CurrencyResponseModel
    {
        public Guid Id { get; set; }
        public string CurrencyCode { get; set; }
        public decimal Rate { get; set; }

    }
}
