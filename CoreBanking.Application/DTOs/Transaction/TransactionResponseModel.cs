namespace CoreBanking.Application.DTOs.Transaction
{
    public class TransactionResponseModel
    {
        public int? Id { get; set; }
        public List<TransactionDetailResponseModel> Details { get; set; }
    }
}
