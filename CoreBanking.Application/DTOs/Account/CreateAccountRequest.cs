namespace CoreBanking.Application.DTOs.Account
{
    public class CreateAccountRequest
    {
        public Guid ClientId { get; set; }
        public string CurrencyCode { get; set; }
    }
}
