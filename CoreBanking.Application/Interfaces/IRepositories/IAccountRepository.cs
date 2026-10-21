using CoreBanking.Domain.Entities.AccountEntities;

namespace CoreBanking.Application.Interfaces.IRepositories
{
    public interface IAccountRepository : IBaseRepository<Account>
    {
        public Task<List<Account>> GetAccountsByClientIdAsync(Guid Id);
        public Task<bool> ClientHasAccountWithCurrencyCodeAsync(Guid id, string code);
        public Task<Account> GetOnlyAccountByIdAsync(Guid id);
    }
}
