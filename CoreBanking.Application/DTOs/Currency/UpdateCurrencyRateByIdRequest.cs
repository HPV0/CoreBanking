namespace CoreBanking.Application.DTOs.Currency
{
    public class UpdateCurrencyRateByIdRequest
    {
        public int Id { get; set; }
        public decimal Rate { get; set; }
    }
}
