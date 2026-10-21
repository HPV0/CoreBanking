namespace CoreBanking.Application.DTOs.Currency
{
    public class UpdateCurrencyRateByIdRequest
    {
        public Guid Id { get; set; }
        public decimal Rate { get; set; }
    }
}
