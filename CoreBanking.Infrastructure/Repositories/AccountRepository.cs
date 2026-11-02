using CoreBanking.Application.Interfaces.IRepositories;
using CoreBanking.Domain.Entities.AccountEntities;
using CoreBanking.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace CoreBanking.Infrastructure.Repositories
{
    public class AccountRepository : BaseRepository<Account>, IAccountRepository
    {
        protected override IQueryable<Account> BaseQuery
        => _dbContext.Accounts.Include(a => a.Balance).Include(a => a.Currency);

        public AccountRepository(ApplicationDbContext db) : base(db) { }

        public async Task<List<Account>> GetAccountsByClientIdAsync(int Id)
        {
            return await BaseQuery.Where(a => a.ClientId == Id).ToListAsync();
        }

        public async Task<bool> ClientHasAccountWithCurrencyCodeAsync(int id, string code)
        {
            var account = await BaseQuery.FirstOrDefaultAsync(a => a.Id == id && a.Currency.CurrencyCode == code);
            return account != null;
        }

        public async Task<Account> GetOnlyAccountByIdAsync(int id)
        {
            return await _dbContext.Accounts.FirstOrDefaultAsync(a => a.Id == id);
        }

        public override void Remove(Account entity)
        => entity.IsActive = false;
    }
}
