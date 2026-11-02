namespace CoreBanking.Application.DTOs.Transaction
{
    public class TransactionDetailResponseModel
    {
        public int SenderId { get; set; }
        public int Id { get; set; }
        public int ReceiverId { get; set; }
        public decimal ExchangeRate { get; set; }
        public decimal Amount { get; set; }
        public int? TransactionId { get; set; }
    }
}
