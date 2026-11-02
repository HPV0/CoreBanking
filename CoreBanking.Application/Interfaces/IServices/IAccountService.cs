using CoreBanking.Application.DTOs.Account;

namespace CoreBanking.Application.Interfaces.IServices
{
    public interface IAccountService
    {
        public Task<AccountResponseModel> CreateAsync(CreateAccountRequest createAccountRequest);
        public Task<List<AccountResponseModel>> GetAccountsByClientIdAsync(int id);
        public Task<AccountResponseModel> GetAccountByIdAsync(int id);
        public Task RemoveAccountByIdAsync(int id);
        public Task<AccountResponseModel> WithdrawCashByAccountIdAsync(int id, decimal amount);
        public Task<AccountResponseModel> DepositCashByAccountIdAsync(int id, decimal amount);
        public Task<List<AccountResponseModel>?> GetAccountsAsync();
    }
}
