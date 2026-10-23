using CoreBanking.Application.DTOs.Transaction;

namespace CoreBanking.Application.Interfaces.IServices
{
    public interface ITransactionService
    {
        public Task<TransactionResponseModel> CreateAsync(CreateTransactionRequest createTransactionRequest);
        public Task<List<AccountTotalWithdrawalsResponseModel>> GetTotalWithdrawalsByAccountsAsync(DateTime timeFrom, DateTime timeTo);
        public Task<List<AccountCashFlowResponseModel>> GetTotalCashFlowsByAccountsAsync(DateTime timeFrom, DateTime timeTo);
        public Task<List<AccountTotalDepositesResponseModel>> GetAccountsTotalDepositBetweenDatesAsync(DateTime timeFrom, DateTime timeTo);
        public Task<TransactionResponseModel> UndoTransactionByIdAsync(Guid id);
        public Task<TransactionResponseModel> GetTransactionByIdAsync(Guid id);
        public Task<List<TransactionResponseModel>> GetAllTransactionsAsync();
    }
}
