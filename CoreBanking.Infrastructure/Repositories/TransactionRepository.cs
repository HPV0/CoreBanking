using CoreBanking.Application.DTOs.Transaction;
using CoreBanking.Application.Exceptions;
using CoreBanking.Application.Interfaces.IRepositories;
using CoreBanking.Domain.Entities.TransactionEntities;
using CoreBanking.Domain.ValueObjects;
using CoreBanking.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace CoreBanking.Infrastructure.Repositories
{
    public class TransactionRepository : BaseRepository<Transaction>, ITransactionRepository
    {
        protected override IQueryable<Transaction> BaseQuery
            => _dbContext.Transactions.Include(t => t.Details);

        protected IQueryable<Transaction> OnlyTransactionQuery
            => _dbContext.Transactions;


        public TransactionRepository(ApplicationDbContext db) : base(db) { }
        
        public async Task<List<AccountTotalDepositesResponseModel>> GetAccountsTotalDepositBetweenDatesAsync(DateTime timeFrom, DateTime timeTo)
        {
            return await _dbContext.Accounts
                .Select(a => new AccountTotalDepositesResponseModel
                {
                    AccountId = a.Id,
                    CurrencyCode = a.Currency.CurrencyCode,
                    
                    TotalDeposites = Money.NormalizeMoneyAmount(
                        _dbContext.Transactions
                            .Where(t => t.InsertedAt >= timeFrom && t.InsertedAt < timeTo)
                            .SelectMany(t => t.Details)
                            .Where(d => d.AccountToId == a.Id)
                            .Sum(d => (decimal?)d.Amount * d.ExchangeRate) ?? 0)
                })
                .ToListAsync();
        }

        public async Task<List<AccountTotalWithdrawalsResponseModel>> GetTotalWithdrawalsByAccountsAsync(DateTime timeFrom, DateTime timeTo)
        {
            return await _dbContext.Accounts
                .Select(a => new AccountTotalWithdrawalsResponseModel
                {
                    AccountId = a.Id,
                    CurrencyCode = a.Currency.CurrencyCode,
                    
                    TotalWithdrawals = Money.NormalizeMoneyAmount(
                    _dbContext.Transactions
                        .Where(t => t.InsertedAt >= timeFrom && t.InsertedAt < timeTo)
                        .SelectMany(t => t.Details)
                        .Where(d => d.AccountFromId == a.Id)
                        .Sum(d => (decimal?)d.Amount) ?? 0)
                })
                .ToListAsync();
        }

        public async Task<Transaction> GetOnlyTransactionWithNoDetailsByIdAsync(Guid id)
        {
            return await OnlyTransactionQuery.FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task SoftDeleteTransactionAsync(Transaction transaction)
        {

            transaction.SoftDelete();

            foreach (var detail in transaction?.Details)
            {
                detail.SoftDelete();
            }

        }

    }
}
