using CoreBanking.Application.DTOs.Account;

namespace CoreBanking.Application.Interfaces.IServices
{
    public interface IAccountService
    {
        public Task<AccountResponseModel> CreateAsync(CreateAccountRequest createAccountRequest);
        public Task<List<AccountResponseModel>> GetAccountsByClientIdAsync(Guid id);
        public Task<AccountResponseModel> GetAccountByIdAsync(Guid id);
        public Task RemoveAccountByIdAsync(Guid id);
        public Task<AccountResponseModel> WithdrawCashByAccountIdAsync(Guid id, decimal amount);
        public Task<AccountResponseModel> DepositCashByAccountIdAsync(Guid id, decimal amount);
        public Task<List<AccountResponseModel>?> GetAccountsAsync();
    }
}
