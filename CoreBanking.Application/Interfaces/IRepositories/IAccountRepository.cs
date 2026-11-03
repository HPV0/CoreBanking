using CoreBanking.Domain.Entities.AccountEntities;

namespace CoreBanking.Application.Interfaces.IRepositories
{
    public interface IAccountRepository : IBaseRepository<Account>
    {
        public Task<List<Account>> GetAccountsByClientIdAsync(int Id);
        public Task<bool> ClientHasAccountWithCurrencyCodeAsync(int id, string code);
        public Task<Account> GetOnlyAccountByIdAsync(int id);
        public Task<Account> GetBankAccountAsync();
    }
}
