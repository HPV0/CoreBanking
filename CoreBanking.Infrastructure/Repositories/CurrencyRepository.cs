using CoreBanking.Application.Interfaces.IRepositories;
using CoreBanking.Domain.Entities.CurrencyEntities;
using CoreBanking.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace CoreBanking.Infrastructure.Repositories
{
    public class CurrencyRepository : BaseRepository<Currency>, ICurrencyRepository
    {
        protected override IQueryable<Currency> BaseQuery
        => _dbContext.Currencies.Include(c => c.Rates);

        protected IQueryable<Currency> CurrentRateQuery
        => _dbContext.Currencies.Include(c => c.Rates
            .Where(r => r.ValidTo == null));

        protected IQueryable<CurrencyRate> CurrencyRateQuery
        => _dbContext.CurrencyRates;

        protected IQueryable<Currency> OnlyCurrencyQuery
        => _dbContext.Currencies;

        public CurrencyRepository(ApplicationDbContext db) : base(db) { }

        public async Task<Currency?> GetCurrencyWithActiveRateByIdAsync(int id)
        {

            var currency = await CurrentRateQuery.Where(c => c.Id == id).FirstOrDefaultAsync();

            return currency;
        }


        public async Task<Currency?> GetCurrencyWithActiveRateByCodeAsync(string code)
        {
            var currency = await CurrentRateQuery.Where(c => c.CurrencyCode == code).FirstOrDefaultAsync();

            return currency;
        }

        public async Task<Currency> GetOnlyCurrencyByIdAsync(int id)
        {
            var currency = await OnlyCurrencyQuery.FirstOrDefaultAsync(c => c.Id == id);
            return currency;
        }

        public async Task<Currency> GetOnlyCurrencyByCodeAsync(string code)
        {
            var currency = await OnlyCurrencyQuery.FirstOrDefaultAsync(c => c.CurrencyCode == code);
            return currency;
        }

        public async Task<CurrencyRate?> GetActiveCurrencyRateByCodeAsync(string code)
        {
            var currency = await _set.FirstOrDefaultAsync(c => c.CurrencyCode == code);

            if (currency == null)
                return null;

            var currencyRate = await CurrencyRateQuery.FirstOrDefaultAsync(cr => cr.CurrencyId == currency.Id);

            return currencyRate;
        }

        public async Task<CurrencyRate?> GetActiveCurrencyRateByIdAsync(int Id)
        {
            var currencyRate = await CurrencyRateQuery.FirstOrDefaultAsync(cr => cr.CurrencyId == Id && cr.ValidTo == null);
            return currencyRate;
        }

        public async Task<bool> CurrencyCodeExist(string code)
        {
            var currencyExist = await OnlyCurrencyQuery.AnyAsync(c => c.CurrencyCode == code);
            return currencyExist;
        }
    }
}
