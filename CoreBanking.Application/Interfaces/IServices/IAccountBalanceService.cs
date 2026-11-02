using CoreBanking.Domain.Entities.AccountEntities;

namespace CoreBanking.Application.Interfaces.IServices
{
    public interface IAccountBalanceService
    {
        public void Deposit(Account account, decimal amount);
        public void Withdraw(Account account, decimal amount);
    }
}
