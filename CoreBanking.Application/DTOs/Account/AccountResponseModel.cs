namespace CoreBanking.Application.DTOs.Account
{
    public class AccountResponseModel
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; }
    }
}
