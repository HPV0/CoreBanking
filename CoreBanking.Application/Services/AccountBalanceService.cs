using CoreBanking.Application.Exceptions;
using CoreBanking.Application.Interfaces.Common;
using CoreBanking.Application.Interfaces.IServices;
using CoreBanking.Domain.Entities.AccountEntities;

namespace CoreBanking.Application.Services
{
    public class AccountBalanceService : IAccountBalanceService
    {
        private IMoneyCalculator _moneyCalculator;

        public AccountBalanceService(IMoneyCalculator moneyCalculator)
        {
            _moneyCalculator = moneyCalculator;
        }

        public void Deposit(Account account, decimal amount)
        {
            if (amount <= 0)
                throw new UnprocessableException("Amount must be positive");

            _moneyCalculator.NormalizeMoneyAmount(ref amount);

            account.Balance.Amount += amount;
        }
        public void Withdraw(Account account, decimal amount)
        {
            if (amount <= 0)
                throw new UnprocessableException("Amount must be positive");

            if (account.Balance.Amount < amount)
                throw new UnprocessableException("Insufficent funds");

            _moneyCalculator.NormalizeMoneyAmount(ref amount);

            account.Balance.Amount -= amount;
        }
    }
}
