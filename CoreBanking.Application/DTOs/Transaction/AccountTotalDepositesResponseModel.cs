namespace CoreBanking.Application.DTOs.Transaction
{
    public class AccountTotalDepositesResponseModel
    {
        public Guid AccountId { get; set; }
        public decimal TotalDeposites { get; set; }
        public string CurrencyCode { get; set; }
    }
}
