namespace CoreBanking.Application.DTOs.Transaction
{
    public class AccountCashFlowResponseModel
    {
        public int Id { get; set; }
        public string CurrencyCode { get; set; }
        public decimal SpendingAmount { get; set; }
        public decimal IncomesAmount { get; set; }
        public decimal BalanceChangeAmount { get; set; }
    }
}
