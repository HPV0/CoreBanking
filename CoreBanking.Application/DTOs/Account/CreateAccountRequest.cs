namespace CoreBanking.Application.DTOs.Account
{
    public class CreateAccountRequest
    {
        public int ClientId { get; set; }
        public string CurrencyCode { get; set; }
    }
}
