using CoreBanking.Application.DTOs.Transaction;
using CoreBanking.Domain.Entities.TransactionEntities;

namespace CoreBanking.Application.Interfaces.IRepositories
{
    public interface ITransactionRepository : IBaseRepository<Transaction>
    {
        public Task<List<AccountTotalDepositesResponseModel>> GetAccountsTotalDepositBetweenDatesAsync(DateTime timeFrom, DateTime timeTo);
        public Task<List<AccountTotalWithdrawalsResponseModel>> GetTotalWithdrawalsByAccountsAsync(DateTime timeFrom, DateTime timeTo);
        public Task<Transaction> GetOnlyTransactionWithNoDetailsByIdAsync(Guid id);
    }
}
