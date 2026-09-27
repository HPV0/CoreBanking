using CoreBanking.Application.DTOs.Transaction;
using CoreBanking.Application.Interfaces.IRepositories;
using CoreBanking.Domain.Entities.AccountEntities;
using CoreBanking.Domain.Entities.CurrencyEntities;
using CoreBanking.Domain.Entities.TransactionEntities;
using CoreBanking.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreBanking.Infrastructure.Repositories
{
    public class TransactionRepository : BaseRepository<Transaction>, ITransactionRepository
    {
        protected override IQueryable<Transaction> BaseQuery
            => _dbContext.Transactions.Include(t => t.Details);

        protected IQueryable<Transaction> TransactionQuery
            => _dbContext.Transactions;
        public TransactionRepository(ApplicationDbContext db) : base(db) { }
        public async Task<List<AccountTotalDepositesResponseModel>> GetAccountsTotalDepositBetweenDatesAsync(DateTime timeFrom, DateTime timeTo)
        {
            return await _dbContext.Accounts
                .Select(a => new AccountTotalDepositesResponseModel
                {
                    AccountId = a.Id,
                    CurrencyCode = a.Currency.CurrencyCode,
                    TotalDeposites = Math.Round( 
                        _dbContext.Transactions
                            .Where(t => t.InsertedAt >= timeFrom && t.InsertedAt < timeTo)
                            .SelectMany(t => t.Details)
                            .Where(d => d.AccountToId == a.Id)
                            .Sum(d => (decimal?)d.Amount * d.ExchangeRate) ?? 0m,
                    2)
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
                    TotalWithdrawals = Math.Round(
                    _dbContext.Transactions
                        .Where(t => t.InsertedAt >= timeFrom && t.InsertedAt < timeTo)
                        .SelectMany(t => t.Details)
                        .Where(d => d.AccountFromId == a.Id)
                        .Sum(d => (decimal?)d.Amount) ?? 0m,
                    2)
                })
                .ToListAsync();
        }

        public async Task<Transaction> GetOnlyTransactionWithNoDetailsByIdAsync(Guid id)
        {
            return await TransactionQuery.FirstOrDefaultAsync(a => a.Id == id);
        }
    }
}
