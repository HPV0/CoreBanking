namespace CoreBanking.Application.DTOs.Transaction
{
    public class CreateTransactionRequest
    {
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public decimal? SenderExchangeRate { get; set; }
        public decimal? ReceiveExchangeRate { get; set; }
        public decimal Amount { get; set; }
    }
}
