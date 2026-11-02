namespace CoreBanking.Application.DTOs.Transaction
{
    public class AccountTotalWithdrawalsResponseModel
    {
        public int AccountId { get; set; }
        public decimal TotalWithdrawals { get; set; }
        public string CurrencyCode { get; set; }
    }
}
